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
using Newtonsoft.Json;
using System.Net.Http;
using Amazon.Runtime.Internal;

namespace Jarvis
{
    public partial class FrmAcceso : Form
    {
        public FrmAcceso()
        {
            InitializeComponent();
        }

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private int photoCount = 0;

        public async void DetectarPersonaEnImagen(string imagePath)
        {
            try
            {
                // Cargar la imagen
                Mat image = Cv2.ImRead(imagePath);

                // Cargar el clasificador Haar Cascade para detección de rostros
                var faceCascade = new CascadeClassifier(@"..\..\haarcascade_frontalface_default.xml");

                // Convertir la imagen a escala de grises para mejorar la detección
                Mat grayImage = new Mat();
                Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);

                // Detectar las caras en la imagen
                Rect[] faces = faceCascade.DetectMultiScale(grayImage, 1.1, 6); //////////////////////

                // Verificar si se encontraron rostros
                if (faces.Length == 0)
                {
                    MessageBox.Show("No se encontró ninguna cara en la imagen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Dibujar cuadros y etiquetas alrededor de cada cara detectada
                foreach (var face in faces)
                {
                    string imageBase64 = ConvertImageToBase64(imagePath);

                    // Enviar la imagen a través de una solicitud HTTP POST
                    string resultado = await SendImageToServerAsync(imageBase64);

                    if (resultado.Equals(null))
                    {
                        MessageBox.Show("Error en el servidor API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Dibujar el cuadro alrededor de la cara
                    Cv2.Rectangle(image, face, new Scalar(52, 219, 124), 2);

                    // Dibujar el fondo del texto (rectángulo)
                    var (textWidth, textHeight) = Cv2.GetTextSize(resultado, HersheyFonts.HersheySimplex, 0.9, 2, out _);
                    Cv2.Rectangle(image, new OpenCvSharp.Point(face.Left, face.Top - textHeight - 10),
                                  new OpenCvSharp.Point(face.Left + textWidth, face.Top), new Scalar(52, 219, 124), Cv2.FILLED);

                    // Dibujar el texto
                    Cv2.PutText(image, resultado, new OpenCvSharp.Point(face.Left, face.Top - 10),
                                HersheyFonts.HersheySimplex, 0.9, new Scalar(255, 255, 255), 2);

                    // Crear la carpeta "FotoFinal" si no existe
                    string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\FotoFinal");
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    // Guardar la imagen con los cuadros dibujados en la carpeta "FotoFinal"
                    string outputPath = Path.Combine(outputDirectory, Path.GetFileName(imagePath));
                    Cv2.ImWrite(outputPath, image);

                    // Detener la cámara antes de ocultar el formulario y abrir uno nuevo
                    if (videoSource != null && videoSource.IsRunning)
                    {
                        videoSource.SignalToStop();
                        videoSource.WaitForStop();
                    }

                    FrmInicio oFormInicio = new FrmInicio();

                    oFormInicio.AsignarInformacion(outputPath, resultado);

                    oFormInicio.Show();

                    this.Hide();

                    break;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se encontró ninguna cara en la imagen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                videoSource = new VideoCaptureDevice(videoDevices[2].MonikerString);
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

        private static string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

        private static async Task<string> SendImageToServerAsync(string imageBase64)
        {
            using (HttpClient client = new HttpClient())
            {
                var imageRequest = new
                {
                    image = imageBase64
                };

                string jsonContent = JsonConvert.SerializeObject(imageRequest);
                StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("http://"+ ModuloIPDeAPI.ObtenerIP + "/api/clasificar_modelo_cara", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

                    return jsonDataConvert["clasificar_modelo_cara"].ToString();
                }
                else
                {
                   return null;
                }
            }
        }
    }
}
