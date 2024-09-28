using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Runtime.CompilerServices;

namespace Jarvis
{
    public partial class FormAcceso : Form
    {
        public FormAcceso()
        {
            InitializeComponent();
        }

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private int photoCount = 0;

        public void DetectarPersonaEnImagen(string imagen)
        {
            Mat image = Cv2.ImRead(imagen);
            List<string> actoresEncontrados = new List<string>();

            var rekognition = new AmazonRekognitionClient(Amazon.RegionEndpoint.USEast1);
            var dynamodb = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1);

            // Cargar la imagen desde un archivo
            var imagenObtenida = System.Drawing.Image.FromFile(imagen);

            using (var stream = new MemoryStream())
            {
                imagenObtenida.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                var imagenBinaria = stream.ToArray();

                // Detectar caras en la imagen
                var respuesta = rekognition.DetectFaces(new DetectFacesRequest
                {
                    Image = new Amazon.Rekognition.Model.Image
                    {
                        Bytes = new MemoryStream(imagenBinaria)
                    }
                });

                // Verificar si se encontraron caras en la imagen
                if (respuesta.FaceDetails.Count == 0)
                {
                    MessageBox.Show($"No se encontro alguna cara reconocible en la imagen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Iterar sobre cada cara detectada
                foreach (var detalleCara in respuesta.FaceDetails)
                {
                    var box = detalleCara.BoundingBox;
                    var width = imagenObtenida.Width;
                    var height = imagenObtenida.Height;
                    var left = (int)(width * box.Left);
                    var top = (int)(height * box.Top);
                    var faceWidth = (int)(width * box.Width);
                    var faceHeight = (int)(height * box.Height);

                    // Recortar la cara detectada
                    var imagenCara = new Bitmap((int)faceWidth, (int)faceHeight);
                    using (Graphics g = Graphics.FromImage(imagenCara))
                    {
                        g.DrawImage(imagenObtenida, new Rectangle(0, 0, (int)faceWidth, (int)faceHeight),
                                    (int)left, (int)top, (int)faceWidth, (int)faceHeight, GraphicsUnit.Pixel);
                    }

                    using (var caraStream = new MemoryStream())
                    {
                        imagenCara.Save(caraStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        var caraBinaria = caraStream.ToArray();

                        SearchFacesByImageResponse segundaRespuesta;
                        try
                        {
                            segundaRespuesta = rekognition.SearchFacesByImage(new SearchFacesByImageRequest
                            {
                                CollectionId = "actores",
                                Image = new Amazon.Rekognition.Model.Image
                                {
                                    Bytes = new MemoryStream(caraBinaria)
                                }
                            });
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show($"No se encontro alguna cara reconocible en la imagen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        bool found = false;
                        foreach (var match in segundaRespuesta.FaceMatches)
                        {
                            //MessageBox.Show($"{match.Face.FaceId}, {match.Face.Confidence}", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            var cara = dynamodb.GetItem(new GetItemRequest
                            {
                                TableName = "face_recognition",
                                Key = new Dictionary<string, AttributeValue>
                            {
                                { "RekognitionId", new AttributeValue { S = match.Face.FaceId } }
                            }
                            });

                            if (cara.Item != null)
                            {
                                Cv2.Rectangle(image, new OpenCvSharp.Point((int)left, (int)top), new OpenCvSharp.Point((int)(left + faceWidth), (int)(top + faceHeight)),
                                              new Scalar(52, 219, 124), 2);
                               // MessageBox.Show($"Persona encontrada: " + cara.Item["FullName"].S, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                actoresEncontrados.Add(cara.Item["FullName"].S);
                                found = true;

                                var (textWidth, textHeight) = Cv2.GetTextSize(cara.Item["FullName"].S, HersheyFonts.HersheySimplex, 0.9, 2, out _);

                                // Dibujar el fondo del texto (rectángulo)
                                Cv2.Rectangle(image, new OpenCvSharp.Point((int)left, (int)(top - textHeight - 10)), new OpenCvSharp.Point((int)(left + textWidth), (int)top),
                                              new Scalar(52, 219, 124), Cv2.FILLED);

                                // Dibujar el texto
                                Cv2.PutText(image, cara.Item["FullName"].S, new OpenCvSharp.Point((int)left, (int)(top - 10)), HersheyFonts.HersheySimplex, 0.9,
                                            new Scalar(255, 255, 255), 2);

                                // Crear la carpeta "FotoFinal" si no existe
                                string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\FotoFinal");
                                if (!Directory.Exists(outputDirectory))
                                {
                                    Directory.CreateDirectory(outputDirectory);
                                }

                                // Guardar la imagen con los rectángulos dibujados en la carpeta "FotoFinal"
                                string outputPath = Path.Combine(outputDirectory, Path.GetFileName(imagen));
                                Cv2.ImWrite(outputPath, image);

                                // Detener la cámara antes de ocultar el formulario y abrir uno nuevo
                                if (videoSource != null && videoSource.IsRunning)
                                {
                                    videoSource.SignalToStop();
                                    videoSource.WaitForStop();
                                }


                                FormInicio oFormInicio = new FormInicio();

                                oFormInicio.AsignarInformacion(outputPath, cara.Item["FullName"].S);

                                oFormInicio.Show();

                                this.Hide();

                                break;
                            }
                        }

                        if (!found)
                        {
                            Cv2.Rectangle(image, new OpenCvSharp.Point((int)left, (int)top), new OpenCvSharp.Point((int)(left + faceWidth), (int)(top + faceHeight)),
                                          new Scalar(219, 52, 82), 2);

                            var (textWidth, textHeight) = Cv2.GetTextSize("Desconocido(a)", HersheyFonts.HersheySimplex, 0.9, 2, out _);

                            // Dibujar el fondo del texto (rectángulo)
                            Cv2.Rectangle(image, new OpenCvSharp.Point((int)left, (int)(top - textHeight - 10)), new OpenCvSharp.Point((int)(left + textWidth), (int)top),
                                          new Scalar(219, 52, 82), Cv2.FILLED);

                            // Dibujar el texto
                            Cv2.PutText(image, "Desconocido(a)", new OpenCvSharp.Point((int)left, (int)(top - 10)), HersheyFonts.HersheySimplex, 0.9,
                                        new Scalar(255, 255, 255), 2);
                            MessageBox.Show($"La persona no es reconocible.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void FormAcceso_Load(object sender, EventArgs e)
        {
            string folderPathCamara = @"..\..\FotoCamara";
            string folderPathFinal= @"..\..\FotoFinal";
            string folderPathGrabacion = @"..\..\Grabacion";

            // Borrar las imágenes existentes en la carpeta
            var existingFilesCamara = Directory.GetFiles(folderPathCamara);
            foreach (var file in existingFilesCamara)
            {
                File.Delete(file);
            }

            // Borrar las imágenes existentes en la carpeta
            var existingFilesFinal = Directory.GetFiles(folderPathFinal);
            foreach (var file in existingFilesFinal)
            {
                File.Delete(file);
            }

            // Borrar las imágenes existentes en la carpeta
            var existingFilesGrabacion = Directory.GetFiles(folderPathGrabacion);
            foreach (var file in existingFilesGrabacion)
            {
                File.Delete(file);
            }

            // Obtener dispositivos de video
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[3].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                videoSource.Start();
            }
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Liberar el bitmap anterior si existe
            if (pictureBoxCamara.Image != null)
            {
                pictureBoxCamara.Image.Dispose();
            }

            // Crear un bitmap con el tamaño del PictureBox
            Bitmap resizedFrame = new Bitmap(pictureBoxCamara.Width, pictureBoxCamara.Height);

            // Dibujar el cuadro de video original en el nuevo bitmap
            using (Graphics g = Graphics.FromImage(resizedFrame))
            {
                g.DrawImage(eventArgs.Frame, 0, 0, resizedFrame.Width, resizedFrame.Height);
            }

            // Mostrar el video en el PictureBox
            pictureBoxCamara.Image = resizedFrame;
        }

        private void FormAcceso_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Detener la cámara al cerrar la aplicación
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }

        private void btnAcceso_Click(object sender, EventArgs e)
        {
            if (pictureBoxCamara.Image != null)
            {
                string folderPath = @"..\..\FotoCamara";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Incrementar el contador de fotos
                photoCount++;

                string filePath = Path.Combine(folderPath, $"FotoTomada_{photoCount}.jpg");
                pictureBoxCamara.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                DetectarPersonaEnImagen(filePath);
            }

        }
    }
}
