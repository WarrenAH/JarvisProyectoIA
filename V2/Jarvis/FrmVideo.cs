using analisisDatos;
using CliWrap;
using CliWrap.EventStream;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jarvis
{
    public partial class FrmVideo : Form
    {
        public FrmVideo()
        {
            InitializeComponent();
            Objetos(false, true);
        }

        private void Objetos(bool primeraSeleccion, bool segundaEleccion)
        {
            lblNombreImagen.Visible = primeraSeleccion;
            btnDerecha.Visible = primeraSeleccion;
            btnIzquierda.Visible = primeraSeleccion;
            pictureBoxResultados.Visible = primeraSeleccion;
            richTxtResultados.Visible = primeraSeleccion;
            txtRutaVideo.Visible = segundaEleccion;
            btnSeleccionar.Visible = segundaEleccion;
            btnIniciar.Visible = segundaEleccion;

            // Asegurarnos de que el formulario ajuste su tamaño al contenido
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        private string[] imageFiles; // Array para almacenar las rutas de las imágenes
        private int currentIndex = 0; // Índice actual para navegar por las imágenes

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de video (*.mp4)|*.mp4;";
                ofd.Title = "Selecciona un archivo de video";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtRutaVideo.Text = ofd.FileName; // Mostrar la ruta en el TextBox
                }
            }
        }

        private void BorrarCarpetaResultados()
        {
            string carpetaResultados = @"..\..\ModeloArmasBombas\resultados";

            // Verificar si la carpeta existe
            if (Directory.Exists(carpetaResultados))
            {
                try
                {
                    // Si hay una imagen cargada en el PictureBox, liberar los recursos
                    if (pictureBoxResultados.Image != null)
                    {
                        pictureBoxResultados.Image.Dispose();
                        pictureBoxResultados.Image = null;
                    }

                    // Borrar todos los archivos en la carpeta
                    foreach (var archivo in Directory.GetFiles(carpetaResultados))
                    {
                        File.Delete(archivo);
                    }

                    // Borrar la carpeta si está vacía
                    Directory.Delete(carpetaResultados);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al borrar la carpeta de resultados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //MessageBox.Show("La carpeta de resultados no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutaVideo.Text) || !File.Exists(txtRutaVideo.Text))
            {
                MessageBox.Show("Por favor, selecciona un archivo de video válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Borra la carpeta resultados si existe
            BorrarCarpetaResultados();

            // Iniciar el script Python
            FrmTextoPython frmTextoPython = new FrmTextoPython();
            frmTextoPython.Show();

            Hide();

            // Definir la ruta del script Python
            string programaSeleccion = @"..\..\ModeloArmasBombas\YOLOV8.py"; // Modifica la ruta si es necesario

            // Ejecutar el script de manera asíncrona
            Task tareaPython = Task.Run(() => frmTextoPython.programa(programaSeleccion, txtRutaVideo.Text));

            // Verificación de los procesos en paralelo
            await verificacion(frmTextoPython, tareaPython);

            frmTextoPython.CerrarFormularioProgramaticamente();
        }

        private async Task verificacion(FrmTextoPython frmTextoPython, Task tareaPython)
        {
            // Esperar a que ambas tareas se completen
            await Task.WhenAll(tareaPython);

            Objetos(true, true);

            MostrarResultadosTxt();

            CargarImagenesResultados();

            // Mostrar la primera imagen tras cargar
            currentIndex = 0; // Reinicia el índice al inicio
            MostrarImagenesResultados();

            Show();
        }

        private void FormVideo_Load(object sender, EventArgs e)
        {
        }

        private void MostrarResultadosTxt()
        {
            // Especifica la ruta del archivo .txt
            string filePath = @"..\..\ModeloArmasBombas\resultados\resultados.txt";

            // Verifica si el archivo existe
            if (File.Exists(filePath))
            {
                try
                {
                    // Lee todo el contenido del archivo
                    string fileContent = File.ReadAllText(filePath);

                    // Muestra el contenido del archivo en el TextBox o RichTextBox
                    richTxtResultados.Text = fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al leer el archivo: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("El archivo no existe.");
            }
        }

        // Cargar las imágenes de la carpeta
        private void CargarImagenesResultados()
        {
            string folderPath = @"..\..\ModeloArmasBombas\resultados"; // Cambia esto a la ruta de tu carpeta
            imageFiles = Directory.GetFiles(folderPath, "*.jpg"); // O usa el filtro que prefieras, como *.png
            if (imageFiles.Length == 0)
            {
                MessageBox.Show("No se encontraron imágenes en la carpeta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Mostrar la imagen y el nombre del archivo
        private void MostrarImagenesResultados()
        {
            if (imageFiles.Length > 0)
            {
                try
                {
                    // Liberar recursos de la imagen anterior si existe
                    if (pictureBoxResultados.Image != null)
                    {
                        pictureBoxResultados.Image.Dispose();
                        pictureBoxResultados.Image = null;
                    }

                    // Cargar la nueva imagen en el PictureBox
                    pictureBoxResultados.Image = Image.FromFile(imageFiles[currentIndex]);

                    // Ajustar la imagen al tamaño del PictureBox
                    pictureBoxResultados.SizeMode = PictureBoxSizeMode.StretchImage;

                    // Mostrar el nombre del archivo
                    lblNombreImagen.Text = Path.GetFileName(imageFiles[currentIndex]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No hay imágenes disponibles para mostrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FormVideo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Leer el ProcessId desde la clase estática
            int processId = ProcessInfo.ProcessId;

            if (processId != -1) // Verificar si hay un ProcessId válido
            {
                try
                {
                    Process processToKill = Process.GetProcessById(processId);
                    processToKill.Kill();
                    //ProcessInfo.ProcessId = -1;
                    //MessageBox.Show($"Proceso con ID {processId} cerrado exitosamente.");
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Error al cerrar el proceso: {ex.Message}");
                }
            }
            else
            {
                //MessageBox.Show("No se ha encontrado un proceso válido o el proceso ya ha finalizado.");
            }

        }

        private void btnDerecha_Click(object sender, EventArgs e)
        {
            if (currentIndex < imageFiles.Length - 1)
            {
                currentIndex++;
                MostrarImagenesResultados();
            }
        }

        private void btnIzquierda_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                MostrarImagenesResultados();
            }
        }
    }
}
