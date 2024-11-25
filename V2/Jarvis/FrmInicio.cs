using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.Lame;
using NAudio.Wave.Compression;
using NAudio.Wave;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;

namespace Jarvis
{
    public partial class FrmInicio : Form
    {
        private WaveInEvent waveSource;
        private WaveFileWriter waveFile;
        private int recordingCount = 0;

        private string folderPath = @"..\..\Grabacion";

        // Declarar una variable estática para controlar la instancia del formulario
        private static FrmVideo frmVideo = null;

        public FrmInicio()
        {
            InitializeComponent();
            EsconderObjetos();
        }

        public void AsignarInformacion(string rutaImagen, string nombreUsuario)
        {
            lblBienvenida.Text = lblBienvenida.Text + " " + nombreUsuario;

            // Cargar una imagen desde un archivo
            pictureBoxFoto.Image = Image.FromFile(rutaImagen);

            // Opcional: Ajustar el tamaño de la imagen para que se ajuste al PictureBox
            pictureBoxFoto.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            btnEnviar.Visible = false;
            lblReconocimiento.Visible = false;
            lblReconocimientoAutomatico.Visible = false;

            // Incrementar el contador de grabaciones
            recordingCount++;

            // Construir la ruta del archivo
            string filePath = Path.Combine(folderPath, $"GrabacionTomada_{recordingCount}.wav");

            waveSource = new WaveInEvent();
            waveSource.WaveFormat = new WaveFormat(44100, 1); // 44.1kHz, mono
            waveSource.DataAvailable += EnInformacionDisponible;
            waveFile = new WaveFileWriter(filePath, waveSource.WaveFormat);
            waveSource.StartRecording();

            btnDetener.Visible = true;
        }

        public void EsconderObjetos()
        {
            btnDetener.Visible = false;
            btnReiniciar.Visible = false;
            btnConsulta.Visible = false;
            lblSeleccion.Visible = false;
            lblInformacion.Visible = false;
            lblReconocimiento.Visible = false;
            lblReconocimientoAutomatico.Visible = false;

            lblCuadro1.Visible = false;
            lblCuadro2.Visible = false;
            lblCuadro3.Visible = false;
            lblCuadro4.Visible = false;
            lblCuadro5.Visible = false;
            lblCuadro6.Visible = false;
            lblCuadro7.Visible = false;
            lblCuadro8.Visible = false;
            lblCuadro9.Visible = false;
            lblCuadro10.Visible = false;
            lblCuadro11.Visible = false;
            lblCuadro12.Visible = false;
            lblCuadro13.Visible = false;
            lblCuadro14.Visible = false;
            lblCuadro15.Visible = false;
            lblCuadro16.Visible = false;
            lblCuadro17.Visible = false;
            lblCuadro18.Visible = false;
            lblCuadro19.Visible = false;

            txtCuadro1.Visible = false;
            txtCuadro2.Visible = false;
            txtCuadro3.Visible = false;
            txtCuadro4.Visible = false;
            txtCuadro5.Visible = false;
            txtCuadro6.Visible = false;
            txtCuadro7.Visible = false;
            txtCuadro8.Visible = false;
            txtCuadro9.Visible = false;
            txtCuadro10.Visible = false;
            txtCuadro11.Visible = false;
            txtCuadro12.Visible = false;
            txtCuadro13.Visible = false;
            txtCuadro14.Visible = false;
            txtCuadro15.Visible = false;
            txtCuadro16.Visible = false;
            txtCuadro17.Visible = false;
            txtCuadro18.Visible = false;
            txtCuadro19.Visible = false;

            txtCuadro1.Text = string.Empty;
            txtCuadro2.Text = string.Empty;
            txtCuadro3.Text = string.Empty;
            txtCuadro4.Text = string.Empty;
            txtCuadro5.Text = string.Empty;
            txtCuadro6.Text = string.Empty;
            txtCuadro7.Text = string.Empty;
            txtCuadro8.Text = string.Empty;
            txtCuadro9.Text = string.Empty;
            txtCuadro10.Text = string.Empty;
            txtCuadro11.Text = string.Empty;
            txtCuadro12.Text = string.Empty;
            txtCuadro13.Text = string.Empty;
            txtCuadro14.Text = string.Empty;
            txtCuadro15.Text = string.Empty;
            txtCuadro16.Text = string.Empty;
            txtCuadro17.Text = string.Empty;
            txtCuadro18.Text = string.Empty;
            txtCuadro19.Text = string.Empty;

            // Asegurarnos de que el formulario ajuste su tamaño al contenido
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        private void EnInformacionDisponible(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        // Método para eliminar tildes
        static string EliminarTildes(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private void ActivarCampos(string palabraFinal)
        {
            if (palabraFinal.Contains("auto"))
            {
                lblSeleccion.Text = "Predecir precio de un automovil";
                lblSeleccion.Visible = true;
                lblCuadro1.Text = "Selling_Price";
                lblCuadro1.Visible = true;
                lblCuadro2.Text = "Kms_Driven";
                lblCuadro2.Visible = true;

                txtCuadro1.Visible = true;
                txtCuadro2.Visible = true;

                btnConsulta.Visible = true;
            }
            else if (palabraFinal.Contains("agua"))
            {
                lblSeleccion.Text = "Predecir precio del aguacate";
                lblSeleccion.Visible = true;
                lblCuadro1.Text = "Type";
                lblCuadro1.Visible = true;
                lblCuadro2.Text = "Month";
                lblCuadro2.Visible = true;
                lblCuadro3.Text = "Day";
                lblCuadro3.Visible = true;
                lblCuadro4.Text = "Year";
                lblCuadro4.Visible = true;

                txtCuadro1.Visible = true;
                txtCuadro2.Visible = true;
                txtCuadro3.Visible = true;
                txtCuadro4.Visible = true;

                btnConsulta.Visible = true;
            }
            else if (palabraFinal.Contains("mas"))
            {
                lblSeleccion.Text = "Predecir la masa corporal";
                lblSeleccion.Visible = true;
                lblCuadro1.Text = "Thigh";
                lblCuadro1.Visible = true;
                lblCuadro2.Text = "Hip";
                lblCuadro2.Visible = true;
                lblCuadro3.Text = "Abdomen";
                lblCuadro3.Visible = true;
                lblCuadro4.Text = "Weight";
                lblCuadro4.Visible = true;

                txtCuadro1.Visible = true;
                txtCuadro2.Visible = true;
                txtCuadro3.Visible = true;
                txtCuadro4.Visible = true;

                btnConsulta.Visible = true;
            }
            else if (palabraFinal.Contains("mon"))
            {
                lblSeleccion.Text = "Predecir precio del bitcoin";
                lblSeleccion.Visible = true;
                lblCuadro1.Text = "Open";
                lblCuadro1.Visible = true;
                lblCuadro2.Text = "High";
                lblCuadro2.Visible = true;
                lblCuadro3.Text = "Low";
                lblCuadro3.Visible = true;

                txtCuadro1.Visible = true;
                txtCuadro2.Visible = true;
                txtCuadro3.Visible = true;

                btnConsulta.Visible = true;
            }
            else if (palabraFinal.Contains("cli"))
            {
                lblSeleccion.Text = "Predecir el indice de precio de los clientes de Walmart";
                lblSeleccion.Visible = true;
                lblCuadro1.Text = "Dept";
                lblCuadro1.Visible = true;
                lblCuadro2.Text = "Month";
                lblCuadro2.Visible = true;

                txtCuadro1.Visible = true;
                txtCuadro2.Visible = true;

                btnConsulta.Visible = true;
            }
            else if (palabraFinal.Contains("vin"))
            {
                lblSeleccion.Text = "Clasificar la calidad del vino";
                lblSeleccion.Visible = true;
                lblCuadro1.Text = "type";
                lblCuadro1.Visible = true;
                lblCuadro2.Text = "fixed acidity";
                lblCuadro2.Visible = true;
                lblCuadro3.Text = "volatile acidity";
                lblCuadro3.Visible = true;
                lblCuadro4.Text = "citric acid";
                lblCuadro4.Visible = true;
                lblCuadro5.Text = "residual sugar";
                lblCuadro5.Visible = true;
                lblCuadro6.Text = "chlorides";
                lblCuadro6.Visible = true;
                lblCuadro7.Text = "free sulfur dioxide";
                lblCuadro7.Visible = true;
                lblCuadro8.Text = "total sulfur dioxide";
                lblCuadro8.Visible = true;
                lblCuadro9.Text = "density";
                lblCuadro9.Visible = true;
                lblCuadro10.Text = "pH";
                lblCuadro10.Visible = true;
                lblCuadro11.Text = "sulphates";
                lblCuadro11.Visible = true;
                lblCuadro12.Text = "alcohol";
                lblCuadro12.Visible = true;

                txtCuadro1.Visible = true;
                txtCuadro2.Visible = true;
                txtCuadro3.Visible = true;
                txtCuadro4.Visible = true;
                txtCuadro5.Visible = true;
                txtCuadro6.Visible = true;
                txtCuadro7.Visible = true;
                txtCuadro8.Visible = true;
                txtCuadro9.Visible = true;
                txtCuadro10.Visible = true;
                txtCuadro11.Visible = true;
                txtCuadro12.Visible = true;

                btnConsulta.Visible = true;

            }
            else if (palabraFinal.Contains("cir") || palabraFinal.Contains("cirr"))
            {
                lblSeleccion.Text = "Clasificar el tipo de cirrosis";
                lblSeleccion.Visible = true;
                lblCuadro1.Text = "N_Days";
                lblCuadro1.Visible = true;
                lblCuadro2.Text = "Status";
                lblCuadro2.Visible = true;
                lblCuadro3.Text = "Drug";
                lblCuadro3.Visible = true;
                lblCuadro4.Text = "Age";
                lblCuadro4.Visible = true;
                lblCuadro5.Text = "Sex";
                lblCuadro5.Visible = true;
                lblCuadro6.Text = "Ascites";
                lblCuadro6.Visible = true;
                lblCuadro7.Text = "Hepatomegaly";
                lblCuadro7.Visible = true;
                lblCuadro8.Text = "Spiders";
                lblCuadro8.Visible = true;
                lblCuadro9.Text = "Edema";
                lblCuadro9.Visible = true;
                lblCuadro10.Text = "Bilirubin";
                lblCuadro10.Visible = true;
                lblCuadro11.Text = "Cholesterol";
                lblCuadro11.Visible = true;
                lblCuadro12.Text = "Albumin";
                lblCuadro12.Visible = true;
                lblCuadro13.Text = "Copper";
                lblCuadro13.Visible = true;
                lblCuadro14.Text = "Alk_Phos";
                lblCuadro14.Visible = true;
                lblCuadro15.Text = "SGOT";
                lblCuadro15.Visible = true;
                lblCuadro16.Text = "Tryglicerides";
                lblCuadro16.Visible = true;
                lblCuadro17.Text = "Platelets";
                lblCuadro17.Visible = true;
                lblCuadro18.Text = "Prothrombin";
                lblCuadro18.Visible = true;

                txtCuadro1.Visible = true;
                txtCuadro2.Visible = true;
                txtCuadro3.Visible = true;
                txtCuadro4.Visible = true;
                txtCuadro5.Visible = true;
                txtCuadro6.Visible = true;
                txtCuadro7.Visible = true;
                txtCuadro8.Visible = true;
                txtCuadro9.Visible = true;
                txtCuadro10.Visible = true;
                txtCuadro11.Visible = true;
                txtCuadro12.Visible = true;
                txtCuadro13.Visible = true;
                txtCuadro14.Visible = true;
                txtCuadro15.Visible = true;
                txtCuadro16.Visible = true;
                txtCuadro17.Visible = true;
                txtCuadro18.Visible = true;

                btnConsulta.Visible = true;

            }
            else if (palabraFinal.Contains("hepa") || palabraFinal.Contains("epa"))
            {
                lblSeleccion.Text = "Clasificar el tipo de hepatitis";
                lblSeleccion.Visible = true;
                lblCuadro1.Text = "Age";
                lblCuadro1.Visible = true;
                lblCuadro2.Text = "Sex";
                lblCuadro2.Visible = true;
                lblCuadro3.Text = "ALB";
                lblCuadro3.Visible = true;
                lblCuadro4.Text = "ALP";
                lblCuadro4.Visible = true;
                lblCuadro5.Text = "ALT";
                lblCuadro5.Visible = true;
                lblCuadro6.Text = "AST";
                lblCuadro6.Visible = true;
                lblCuadro7.Text = "BIL";
                lblCuadro7.Visible = true;
                lblCuadro8.Text = "CHE";
                lblCuadro8.Visible = true;
                lblCuadro9.Text = "CHOL";
                lblCuadro9.Visible = true;
                lblCuadro10.Text = "CREA";
                lblCuadro10.Visible = true;
                lblCuadro11.Text = "GGT";
                lblCuadro11.Visible = true;
                lblCuadro12.Text = "PROT";
                lblCuadro12.Visible = true;

                txtCuadro1.Visible = true;
                txtCuadro2.Visible = true;
                txtCuadro3.Visible = true;
                txtCuadro4.Visible = true;
                txtCuadro5.Visible = true;
                txtCuadro6.Visible = true;
                txtCuadro7.Visible = true;
                txtCuadro8.Visible = true;
                txtCuadro9.Visible = true;
                txtCuadro10.Visible = true;
                txtCuadro11.Visible = true;
                txtCuadro12.Visible = true;

                btnConsulta.Visible = true;

            }
            else if (palabraFinal.Contains("acc") || palabraFinal.Contains("ac"))
            {
                lblSeleccion.Text = "Clasificar si un paciente tendra un accidente cerebro-vascular";
                lblSeleccion.Visible = true;
                lblCuadro1.Text = "gender";
                lblCuadro1.Visible = true;
                lblCuadro2.Text = "age";
                lblCuadro2.Visible = true;
                lblCuadro3.Text = "hypertension";
                lblCuadro3.Visible = true;
                lblCuadro4.Text = "heart_disease";
                lblCuadro4.Visible = true;
                lblCuadro5.Text = "ever_married";
                lblCuadro5.Visible = true;
                lblCuadro6.Text = "work_type";
                lblCuadro6.Visible = true;
                lblCuadro7.Text = "Residence_type";
                lblCuadro7.Visible = true;
                lblCuadro8.Text = "avg_glucose_level";
                lblCuadro8.Visible = true;
                lblCuadro9.Text = "bmi";
                lblCuadro9.Visible = true;
                lblCuadro10.Text = "smoking_status";
                lblCuadro10.Visible = true;

                txtCuadro1.Visible = true;
                txtCuadro2.Visible = true;
                txtCuadro3.Visible = true;
                txtCuadro4.Visible = true;
                txtCuadro5.Visible = true;
                txtCuadro6.Visible = true;
                txtCuadro7.Visible = true;
                txtCuadro8.Visible = true;
                txtCuadro9.Visible = true;
                txtCuadro10.Visible = true;

                btnConsulta.Visible = true;
            }
            else
            {
                lblSeleccion.Text = "Clasificar si un cliente se pasara de compañia telefonica";
                lblSeleccion.Visible = true;
                lblCuadro1.Text = "gender";
                lblCuadro1.Visible = true;
                lblCuadro2.Text = "SeniorCitizen";
                lblCuadro2.Visible = true;
                lblCuadro3.Text = "Partner";
                lblCuadro3.Visible = true;
                lblCuadro4.Text = "Dependents";
                lblCuadro4.Visible = true;
                lblCuadro5.Text = "tenure";
                lblCuadro5.Visible = true;
                lblCuadro6.Text = "PhoneService";
                lblCuadro6.Visible = true;
                lblCuadro7.Text = "MultipleLines";
                lblCuadro7.Visible = true;
                lblCuadro8.Text = "InternetService";
                lblCuadro8.Visible = true;
                lblCuadro9.Text = "OnlineSecurity";
                lblCuadro9.Visible = true;
                lblCuadro10.Text = "OnlineBackup";
                lblCuadro10.Visible = true;
                lblCuadro11.Text = "DeviceProtection";
                lblCuadro11.Visible = true;
                lblCuadro12.Text = "TechSupport";
                lblCuadro12.Visible = true;
                lblCuadro13.Text = "StreamingTV";
                lblCuadro13.Visible = true;
                lblCuadro14.Text = "StreamingMovies";
                lblCuadro14.Visible = true;
                lblCuadro15.Text = "Contract";
                lblCuadro15.Visible = true;
                lblCuadro16.Text = "PaperlessBilling";
                lblCuadro16.Visible = true;
                lblCuadro17.Text = "PaymentMethod";
                lblCuadro17.Visible = true;
                lblCuadro18.Text = "MonthlyCharges";
                lblCuadro18.Visible = true;
                lblCuadro19.Text = "TotalCharges";
                lblCuadro19.Visible = true;

                txtCuadro1.Visible = true;
                txtCuadro2.Visible = true;
                txtCuadro3.Visible = true;
                txtCuadro4.Visible = true;
                txtCuadro5.Visible = true;
                txtCuadro6.Visible = true;
                txtCuadro7.Visible = true;
                txtCuadro8.Visible = true;
                txtCuadro9.Visible = true;
                txtCuadro10.Visible = true;
                txtCuadro11.Visible = true;
                txtCuadro12.Visible = true;
                txtCuadro13.Visible = true;
                txtCuadro14.Visible = true;
                txtCuadro15.Visible = true;
                txtCuadro16.Visible = true;
                txtCuadro17.Visible = true;
                txtCuadro18.Visible = true;
                txtCuadro19.Visible = true;

                btnConsulta.Visible = true;
            }

            // Asegurarnos de que el formulario ajuste su tamaño al contenido
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        private async Task ConsultarModeloAsync()
        {
            if (lblSeleccion.Text.Equals("Predecir precio de un automovil"))
            {
                // URL del endpoint
                string url = "http://"+ ModuloIPDeAPI.ObtenerIP +"/api/predecir_automovil";

                // Crear un objeto con los datos a enviar
                var datos = new Dictionary<string, object>
                {
                    { "Selling_Price", double.Parse(txtCuadro1.Text) },
                    { "Kms_Driven", double.Parse(txtCuadro2.Text) }
                };

                // Serializar el objeto a JSON
                string jsonData = JsonConvert.SerializeObject(datos);

                // Crear una instancia de HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Configurar el contenido de la solicitud
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Hacer la solicitud POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar el JSON a un diccionario
                        var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                        lblInformacion.Text = "Su precio es de : " + jsonDataConvert["prediccion_automovil"].ToString();
                        lblInformacion.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show($"Error en la solicitud: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else if (lblSeleccion.Text.Equals("Clasificar la calidad del vino"))
            {
                // URL del endpoint
                string url = "http://"+ ModuloIPDeAPI.ObtenerIP +"/api/clasificar_calidad_vino";

                // Crear un objeto con los datos a enviar
                var datos = new Dictionary<string, object>
                {
                    { "type", double.Parse(txtCuadro1.Text) },
                    { "fixed acidity", double.Parse(txtCuadro2.Text) },
                    { "volatile acidity", double.Parse(txtCuadro3.Text) },
                    { "citric acid", double.Parse(txtCuadro4.Text) },
                    { "residual sugar", double.Parse(txtCuadro5.Text) },
                    { "chlorides", double.Parse(txtCuadro6.Text) },
                    { "free sulfur dioxide", double.Parse(txtCuadro7.Text) },
                    { "total sulfur dioxide", double.Parse(txtCuadro8.Text) },
                    { "density", double.Parse(txtCuadro9.Text) },
                    { "pH", double.Parse(txtCuadro10.Text) },
                    { "sulphates", double.Parse(txtCuadro11.Text) },
                    { "alcohol", double.Parse(txtCuadro12.Text) }
                };

                // Serializar el objeto a JSON
                string jsonData = JsonConvert.SerializeObject(datos);

                // Crear una instancia de HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Configurar el contenido de la solicitud
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Hacer la solicitud POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar el JSON a un diccionario
                        var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                        lblInformacion.Text = "Su clasificacion es : " + jsonDataConvert["clasificacion_calidad_vino"].ToString();
                        lblInformacion.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show($"Error en la solicitud: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else if (lblSeleccion.Text.Equals("Predecir precio del aguacate"))
            {
                // URL del endpoint
                string url = "http://"+ ModuloIPDeAPI.ObtenerIP +"/api/predecir_aguacate";

                // Crear un objeto con los datos a enviar
                var datos = new Dictionary<string, object>
                {
                    { "Type", double.Parse(txtCuadro1.Text) },
                    { "Month", double.Parse(txtCuadro2.Text) },
                    { "Day", double.Parse(txtCuadro3.Text) },
                    { "Year", double.Parse(txtCuadro4.Text) }
                };

                // Serializar el objeto a JSON
                string jsonData = JsonConvert.SerializeObject(datos);

                // Crear una instancia de HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Configurar el contenido de la solicitud
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Hacer la solicitud POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar el JSON a un diccionario
                        var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                        lblInformacion.Text = "Su precio es de : " + jsonDataConvert["prediccion_aguacate"].ToString();
                        lblInformacion.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show($"Error en la solicitud: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (lblSeleccion.Text.Equals("Clasificar el tipo de cirrosis"))
            {
                // URL del endpoint
                string url = "http://"+ ModuloIPDeAPI.ObtenerIP +"/api/clasificar_tipo_cirrosis";

                // Crear un objeto con los datos a enviar
                var datos = new Dictionary<string, object>
                {
                    { "N_Days", double.Parse(txtCuadro1.Text) },
                    { "Status", double.Parse(txtCuadro2.Text) },
                    { "Drug", double.Parse(txtCuadro3.Text) },
                    { "Age", double.Parse(txtCuadro4.Text) },
                    { "Sex", double.Parse(txtCuadro5.Text) },
                    { "Ascites", double.Parse(txtCuadro6.Text) },
                    { "Hepatomegaly", double.Parse(txtCuadro7.Text) },
                    { "Spiders", double.Parse(txtCuadro8.Text) },
                    { "Edema", double.Parse(txtCuadro9.Text) },
                    { "Bilirubin", double.Parse(txtCuadro10.Text) },
                    { "Cholesterol", double.Parse(txtCuadro11.Text) },
                    { "Albumin", double.Parse(txtCuadro12.Text) },
                    { "Copper", double.Parse(txtCuadro13.Text) },
                    { "Alk_Phos", double.Parse(txtCuadro14.Text) },
                    { "SGOT", double.Parse(txtCuadro15.Text) },
                    { "Tryglicerides", double.Parse(txtCuadro16.Text) },
                    { "Platelets", double.Parse(txtCuadro17.Text) },
                    { "Prothrombin", double.Parse(txtCuadro18.Text) }
                };

                // Serializar el objeto a JSON
                string jsonData = JsonConvert.SerializeObject(datos);

                // Crear una instancia de HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Configurar el contenido de la solicitud
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Hacer la solicitud POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar el JSON a un diccionario
                        var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                        lblInformacion.Text = "Su clasificacion es : " + jsonDataConvert["clasificacion_tipo_cirrosis"].ToString();
                        lblInformacion.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show($"Error en la solicitud: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (lblSeleccion.Text.Equals("Clasificar el tipo de hepatitis")) 
            {
                // URL del endpoint
                string url = "http://"+ ModuloIPDeAPI.ObtenerIP +"/api/clasificar_tipo_hepatitis";

                // Crear un objeto con los datos a enviar
                var datos = new Dictionary<string, object>
                {
                    { "Age", double.Parse(txtCuadro1.Text) },
                    { "Sex", double.Parse(txtCuadro2.Text) },
                    { "ALB", double.Parse(txtCuadro3.Text) },
                    { "ALP", double.Parse(txtCuadro4.Text) },
                    { "ALT", double.Parse(txtCuadro5.Text) },
                    { "AST", double.Parse(txtCuadro6.Text) },
                    { "BIL", double.Parse(txtCuadro7.Text) },
                    { "CHE", double.Parse(txtCuadro8.Text) },
                    { "CHOL", double.Parse(txtCuadro9.Text) },
                    { "CREA", double.Parse(txtCuadro10.Text) },
                    { "GGT", double.Parse(txtCuadro11.Text) },
                    { "PROT", double.Parse(txtCuadro12.Text) }
                };

                // Serializar el objeto a JSON
                string jsonData = JsonConvert.SerializeObject(datos);

                // Crear una instancia de HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Configurar el contenido de la solicitud
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Hacer la solicitud POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar el JSON a un diccionario
                        var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                        lblInformacion.Text = "Su clasificacion es : " + jsonDataConvert["clasificacion_tipo_hepatitis"].ToString();
                        lblInformacion.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show($"Error en la solicitud: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (lblSeleccion.Text.Equals("Predecir la masa corporal")) 
            {
                // URL del endpoint
                string url = "http://"+ ModuloIPDeAPI.ObtenerIP +"/api/predecir_masa_corporal";

                // Crear un objeto con los datos a enviar
                var datos = new Dictionary<string, object>
                {
                    { "Thigh", double.Parse(txtCuadro1.Text) },
                    { "Hip", double.Parse(txtCuadro2.Text) },
                    { "Abdomen", double.Parse(txtCuadro3.Text) },
                    { "Weight", double.Parse(txtCuadro4.Text) }
                };

                // Serializar el objeto a JSON
                string jsonData = JsonConvert.SerializeObject(datos);

                // Crear una instancia de HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Configurar el contenido de la solicitud
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Hacer la solicitud POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar el JSON a un diccionario
                        var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                        lblInformacion.Text = "Su precio es de : " + jsonDataConvert["prediccion_masa_corporal"].ToString();
                        lblInformacion.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show($"Error en la solicitud: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (lblSeleccion.Text.Equals("Clasificar si un paciente tendra un accidente cerebro-vascular")) 
            {
                // URL del endpoint
                string url = "http://"+ ModuloIPDeAPI.ObtenerIP +"/api/clasificar_accidente_cerebro_vascular";

                // Crear un objeto con los datos a enviar
                var datos = new Dictionary<string, object>
                {
                    { "gender", double.Parse(txtCuadro1.Text) },
                    { "age", double.Parse(txtCuadro2.Text) },
                    { "hypertension", double.Parse(txtCuadro3.Text) },
                    { "heart_disease", double.Parse(txtCuadro4.Text) },
                    { "ever_married", double.Parse(txtCuadro5.Text) },
                    { "work_type", double.Parse(txtCuadro6.Text) },
                    { "Residence_type", double.Parse(txtCuadro7.Text) },
                    { "avg_glucose_level", double.Parse(txtCuadro8.Text) },
                    { "bmi", double.Parse(txtCuadro9.Text) },
                    { "smoking_status", double.Parse(txtCuadro10.Text) }
                };

                // Serializar el objeto a JSON
                string jsonData = JsonConvert.SerializeObject(datos);

                // Crear una instancia de HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Configurar el contenido de la solicitud
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Hacer la solicitud POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar el JSON a un diccionario
                        var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                        lblInformacion.Text = "Su clasificacion es : " + jsonDataConvert["clasificacion_accidente_cerebro_vascular"].ToString();
                        lblInformacion.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show($"Error en la solicitud: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (lblSeleccion.Text.Equals("Clasificar si un cliente se pasara de compañia telefonica")) 
            {
                // URL del endpoint
                string url = "http://"+ ModuloIPDeAPI.ObtenerIP +"/api/clasificar_cliente_telefonica";

                // Crear un objeto con los datos a enviar
                var datos = new Dictionary<string, object>
                {
                    { "gender", double.Parse(txtCuadro1.Text) },
                    { "SeniorCitizen", double.Parse(txtCuadro2.Text) },
                    { "Partner", double.Parse(txtCuadro3.Text) },
                    { "Dependents", double.Parse(txtCuadro4.Text) },
                    { "tenure", double.Parse(txtCuadro5.Text) },
                    { "PhoneService", double.Parse(txtCuadro6.Text) },
                    { "MultipleLines", double.Parse(txtCuadro7.Text) },
                    { "InternetService", double.Parse(txtCuadro8.Text) },
                    { "OnlineSecurity", double.Parse(txtCuadro9.Text) },
                    { "OnlineBackup", double.Parse(txtCuadro10.Text) },
                    { "DeviceProtection", double.Parse(txtCuadro11.Text) },
                    { "TechSupport", double.Parse(txtCuadro12.Text) },
                    { "StreamingTV", double.Parse(txtCuadro13.Text) },
                    { "StreamingMovies", double.Parse(txtCuadro14.Text) },
                    { "Contract", double.Parse(txtCuadro15.Text) },
                    { "PaperlessBilling", double.Parse(txtCuadro16.Text) },
                    { "PaymentMethod", double.Parse(txtCuadro17.Text) },
                    { "MonthlyCharges", double.Parse(txtCuadro18.Text) },
                    { "TotalCharges", double.Parse(txtCuadro19.Text) }
                };

                // Serializar el objeto a JSON
                string jsonData = JsonConvert.SerializeObject(datos);

                // Crear una instancia de HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Configurar el contenido de la solicitud
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Hacer la solicitud POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar el JSON a un diccionario
                        var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                        lblInformacion.Text = "Su clasificacion es : " + jsonDataConvert["clasificacion_cliente_telefonica"].ToString();
                        lblInformacion.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show($"Error en la solicitud: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (lblSeleccion.Text.Equals("Predecir precio del bitcoin")) 
            {
                // URL del endpoint
                string url = "http://"+ ModuloIPDeAPI.ObtenerIP +"/api/predecir_bitcoin";

                // Crear un objeto con los datos a enviar
                var datos = new Dictionary<string, object>
                {
                    { "Open", double.Parse(txtCuadro1.Text) },
                    { "High", double.Parse(txtCuadro2.Text) },
                    { "Low", double.Parse(txtCuadro3.Text) }
                };

                // Serializar el objeto a JSON
                string jsonData = JsonConvert.SerializeObject(datos);

                // Crear una instancia de HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Configurar el contenido de la solicitud
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Hacer la solicitud POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar el JSON a un diccionario
                        var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                        lblInformacion.Text = "Su precio es de : " + jsonDataConvert["prediccion_bitcoin"].ToString();
                        lblInformacion.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show($"Error en la solicitud: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                // URL del endpoint
                string url = "http://"+ ModuloIPDeAPI.ObtenerIP +"/api/predecir_indice_precio_walmart";

                // Crear un objeto con los datos a enviar
                var datos = new Dictionary<string, object>
                {
                    { "Dept", double.Parse(txtCuadro1.Text) },
                    { "Month", double.Parse(txtCuadro2.Text) }
                };

                // Serializar el objeto a JSON
                string jsonData = JsonConvert.SerializeObject(datos);

                // Crear una instancia de HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Configurar el contenido de la solicitud
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Hacer la solicitud POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar el JSON a un diccionario
                        var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                        lblInformacion.Text = "Su precio es de : " + jsonDataConvert["prediccion_indice_precio_walmart"].ToString();
                        lblInformacion.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show($"Error en la solicitud: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            // Asegurarnos de que el formulario ajuste su tamaño al contenido
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        private void txtCuadro1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro1.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro2.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro3.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro4.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro5_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro5.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro6_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro6.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro7_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro7.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro8_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro8.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro9_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro9.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro10_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro10.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro11_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro11.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro12_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro12.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro13_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro13.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro14_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro14.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro15_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro15.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro16_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro16.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro17_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro17.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro18_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro18.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCuadro19_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, punto y borrar (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permitir solo un punto
            if (e.KeyChar == ',' && txtCuadro19.Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void FormInicio_Load(object sender, EventArgs e)
        {
            string folderPath = @"..\..\Grabacion";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

        }

        private async void btnDetener_ClickAsync(object sender, EventArgs e)
        {
            btnDetener.Visible = false;

            waveSource.StopRecording();
            waveSource.Dispose();
            waveFile.Close();
            waveFile.Dispose();

            string wavPath = Path.Combine(folderPath, $"GrabacionTomada_{recordingCount}.wav");

            // Convertir el archivo WAV a base64
            byte[] audioBytes = File.ReadAllBytes(wavPath);
            string audioBase64 = Convert.ToBase64String(audioBytes);

            try
            {
                // Esperar el resultado del análisis
                string resultado = await AnalisisAudio(audioBase64);

                lblReconocimiento.Visible = true;

                lblReconocimientoAutomatico.Text = resultado;

                lblReconocimientoAutomatico.Visible = true;

                resultado = resultado.ToLower();

                resultado = EliminarTildes(resultado);

                string[] palabras = resultado.Split(new char[] { ' ', ',', '.', '¡', '!', '¿', '?' }, StringSplitOptions.RemoveEmptyEntries);

                if (palabras[0].Contains("auto") || palabras[0].Contains("vin") || palabras[0].Contains("agua") || palabras[0].Contains("cirr") || 
                    palabras[0].Contains("cir") || palabras[0].Contains("hepa") ||  palabras[0].Contains("mas") || palabras[0].Contains("ac")
                    || palabras[0].Contains("acc") || palabras[0].Contains("comp") || palabras[0].Contains("mon") || palabras[0].Contains("cli")
                    || palabras[0].Contains("epa") || palabras[0].Contains("co"))
                {
                    btnReiniciar.Visible = true;
                    ActivarCampos(palabras[0]);

                }
                else
                {
                    //MessageBox.Show($"No se reconoce la instruccion solicitada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    btnEnviar.Visible = true;

                    return;
                }
            }
            catch
            {
                //MessageBox.Show($"No se reconoce la instruccion solicitada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnEnviar.Visible = true;
            }
        }

        private static async Task<string> AnalisisAudio(string audioBase64)
        {
            string apiUrl = "http://" + ModuloIPDeAPI.ObtenerIP + "/api/reconocimiento_modelo_voz"; // URL de la API

            using (HttpClient client = new HttpClient())
            {
                // Crear el objeto JSON que será enviado
                var audioRequest = new
                {
                    audio = audioBase64
                };

                // Serializar el objeto a JSON
                string jsonContent = JsonConvert.SerializeObject(audioRequest);

                // Crear el contenido HTTP con el tipo "application/json"
                StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Realizar la solicitud POST a la API
                HttpResponseMessage response = await client.PostAsync(apiUrl, httpContent);

                // Verificar si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Deserializar el contenido de la respuesta JSON en un diccionario
                    var jsonDataConvert = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

                    // Retornar el resultado de la predicción
                    return jsonDataConvert["reconocimiento_modelo_voz"].ToString(); // Asegúrate de que la clave coincida con la respuesta de tu API
                }
                else
                {
                    // En caso de error, retornar null o un mensaje adecuado
                    return null;
                }
            }
        }


        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            EsconderObjetos();

            btnEnviar.Visible = true;

            // Asegurarnos de que el formulario ajuste su tamaño al contenido
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        private async void btnConsulta_ClickAsync(object sender, EventArgs e)
        {
            await ConsultarModeloAsync();
        }

        private void FormInicio_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void lblBienvenida_Click(object sender, EventArgs e)
        {

        }

        private void btnArmasBombas_Click(object sender, EventArgs e)
        {
            // Verificar si la ventana ya está abierta
            if (frmVideo == null || frmVideo.IsDisposed)
            {
                // Si no está abierta o ha sido cerrada, creamos una nueva instancia
                frmVideo = new FrmVideo();
                frmVideo.Show();
            }
            else
            {
                // Si ya está abierta, solo activamos la ventana
                frmVideo.Activate();
            }

        }
    }
}
