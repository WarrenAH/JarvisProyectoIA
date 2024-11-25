using CliWrap.EventStream;
using CliWrap;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jarvis;

namespace analisisDatos
{
    public partial class FrmTextoPython : Form
    {
        public FrmTextoPython()
        {
            seleccion = 0;
            InitializeComponent();
        }

        static bool confirmacion = false;
        public int seleccion { get; private set; }

        // Variable para almacenar el proceso
        private Process pythonProcess;

        private bool cierreProgramatico = false;

        public void CerrarFormularioProgramaticamente()
        {
            cierreProgramatico = true;
            this.Close(); // Cerrar desde código sin confirmar.
        }

        private void FrmTextoPython_Load(object sender, EventArgs e)
        {
            // C# 7.3
        }

        public async Task programa(string archivoPy, string videoPath)
        {
            var cmd = Cli.Wrap(@"C:\Users\playa\Desktop\Proyecto 2\venv\Scripts\python.exe")
                .WithArguments(args => args
                    .Add(@"..\..\ModeloArmasBombas\YOLOV8.py")
                    .Add(videoPath))
                .WithValidation(CommandResultValidation.None); // Desactiva la validación del resultado

            await foreach (var cmdEvent in cmd.ListenAsync())
            {
                switch (cmdEvent)
                {
                    case StartedCommandEvent started:
                        pythonProcess = Process.GetProcessById(started.ProcessId); // Guarda el proceso
                        richtxtPython.BeginInvoke((Action)(() => richtxtPython.AppendText($"Proceso iniciado; ID: {started.ProcessId}\n\n")));

                        // Guardar el ProcessId en la clase estática
                        ProcessInfo.ProcessId = started.ProcessId;
                        break;

                    case StandardOutputCommandEvent stdOut:
                        richtxtPython.BeginInvoke((Action)(() => richtxtPython.AppendText(stdOut.Text + "\n")));
                        break;

                    case StandardErrorCommandEvent stdErr:
                        richtxtPython.BeginInvoke((Action)(() => richtxtPython.AppendText($"Error: {stdErr.Text}\n")));
                        break;

                    case ExitedCommandEvent exited:
                        pythonProcess = null; // Limpia la referencia al proceso
                        richtxtPython.BeginInvoke((Action)(() => richtxtPython.AppendText($"\nProceso finalizado; Código: {exited.ExitCode}\n")));
                        break;
                }
            }

            seleccion = 1;
        }

        private Task<DialogResult> ConfirmacionSalida()
        {
            return Task.Run(() =>
            {
                return MessageBox.Show("¿Estás seguro de que deseas salir? Se cerrará la aplicación totalmente.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            });
        }

        private async void FrmTextoPython_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cierreProgramatico)
            {
                // Si el cierre es programático, no mostrar la confirmación
                if (pythonProcess != null && !pythonProcess.HasExited)
                {
                    pythonProcess.Kill();
                    pythonProcess = null; // Limpia la referencia al proceso
                }
                return; // Se sale sin mostrar el cuadro de confirmación.
            }

            // Solo mostrar la confirmación si el cierre es por acción del usuario
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!confirmacion)
                {
                    e.Cancel = true; // Cancela el cierre del formulario mientras mostramos el cuadro de diálogo.

                    DialogResult result = await ConfirmacionSalida();

                    if (result == DialogResult.Yes)
                    {
                        confirmacion = true; // Marca que ya se ha mostrado el cuadro de diálogo.

                        // Finaliza el proceso de Python si está corriendo
                        if (pythonProcess != null && !pythonProcess.HasExited)
                        {
                            pythonProcess.Kill();
                            pythonProcess = null; // Limpia la referencia al proceso
                        }

                        Close(); // Cierra el formulario después de la confirmación
                    }
                }
            }
        }

        private void FrmTextoPython_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (pythonProcess != null && !pythonProcess.HasExited)
            {
                pythonProcess.Kill();
                pythonProcess = null; // Limpia la referencia al proceso
            }
        }

        // Manejo global de cierre de la aplicación
        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            // Si el proceso de Python sigue corriendo, lo mata.
            if (pythonProcess != null && !pythonProcess.HasExited)
            {
                pythonProcess.Kill();
                pythonProcess = null; // Limpia la referencia al proceso
            }
        }
    }
}
