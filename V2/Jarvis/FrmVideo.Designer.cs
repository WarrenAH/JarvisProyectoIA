namespace Jarvis
{
    partial class FrmVideo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSeleccionar = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.txtRutaVideo = new System.Windows.Forms.TextBox();
            this.richTxtResultados = new System.Windows.Forms.RichTextBox();
            this.btnIzquierda = new System.Windows.Forms.Button();
            this.btnDerecha = new System.Windows.Forms.Button();
            this.lblNombreImagen = new System.Windows.Forms.Label();
            this.pictureBoxResultados = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResultados)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSeleccionar
            // 
            this.btnSeleccionar.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionar.Location = new System.Drawing.Point(10, 9);
            this.btnSeleccionar.Name = "btnSeleccionar";
            this.btnSeleccionar.Size = new System.Drawing.Size(236, 43);
            this.btnSeleccionar.TabIndex = 5;
            this.btnSeleccionar.Text = "SELECCIONAR";
            this.btnSeleccionar.UseVisualStyleBackColor = true;
            this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnIniciar
            // 
            this.btnIniciar.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciar.Location = new System.Drawing.Point(652, 9);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(134, 43);
            this.btnIniciar.TabIndex = 6;
            this.btnIniciar.Text = "INICIAR";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // txtRutaVideo
            // 
            this.txtRutaVideo.Enabled = false;
            this.txtRutaVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRutaVideo.Location = new System.Drawing.Point(252, 12);
            this.txtRutaVideo.Name = "txtRutaVideo";
            this.txtRutaVideo.Size = new System.Drawing.Size(305, 38);
            this.txtRutaVideo.TabIndex = 7;
            // 
            // richTxtResultados
            // 
            this.richTxtResultados.Location = new System.Drawing.Point(794, 12);
            this.richTxtResultados.Name = "richTxtResultados";
            this.richTxtResultados.Size = new System.Drawing.Size(230, 377);
            this.richTxtResultados.TabIndex = 51;
            this.richTxtResultados.Text = "";
            // 
            // btnIzquierda
            // 
            this.btnIzquierda.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIzquierda.Location = new System.Drawing.Point(692, 392);
            this.btnIzquierda.Name = "btnIzquierda";
            this.btnIzquierda.Size = new System.Drawing.Size(45, 43);
            this.btnIzquierda.TabIndex = 58;
            this.btnIzquierda.Text = "🠔";
            this.btnIzquierda.UseVisualStyleBackColor = true;
            this.btnIzquierda.Click += new System.EventHandler(this.btnIzquierda_Click);
            // 
            // btnDerecha
            // 
            this.btnDerecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDerecha.Location = new System.Drawing.Point(743, 392);
            this.btnDerecha.Name = "btnDerecha";
            this.btnDerecha.Size = new System.Drawing.Size(45, 43);
            this.btnDerecha.TabIndex = 57;
            this.btnDerecha.Text = "🠖";
            this.btnDerecha.UseVisualStyleBackColor = true;
            this.btnDerecha.Click += new System.EventHandler(this.btnDerecha_Click);
            // 
            // lblNombreImagen
            // 
            this.lblNombreImagen.AutoSize = true;
            this.lblNombreImagen.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreImagen.Location = new System.Drawing.Point(12, 392);
            this.lblNombreImagen.Name = "lblNombreImagen";
            this.lblNombreImagen.Size = new System.Drawing.Size(118, 33);
            this.lblNombreImagen.TabIndex = 56;
            this.lblNombreImagen.Text = "Imagen";
            // 
            // pictureBoxResultados
            // 
            this.pictureBoxResultados.Location = new System.Drawing.Point(12, 64);
            this.pictureBoxResultados.Name = "pictureBoxResultados";
            this.pictureBoxResultados.Size = new System.Drawing.Size(776, 325);
            this.pictureBoxResultados.TabIndex = 60;
            this.pictureBoxResultados.TabStop = false;
            // 
            // FrmVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 443);
            this.Controls.Add(this.pictureBoxResultados);
            this.Controls.Add(this.btnIzquierda);
            this.Controls.Add(this.btnDerecha);
            this.Controls.Add(this.lblNombreImagen);
            this.Controls.Add(this.richTxtResultados);
            this.Controls.Add(this.txtRutaVideo);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.btnSeleccionar);
            this.Name = "FrmVideo";
            this.Text = "FormVideo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormVideo_FormClosing);
            this.Load += new System.EventHandler(this.FormVideo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResultados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSeleccionar;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.TextBox txtRutaVideo;
        private System.Windows.Forms.RichTextBox richTxtResultados;
        private System.Windows.Forms.Button btnIzquierda;
        private System.Windows.Forms.Button btnDerecha;
        private System.Windows.Forms.Label lblNombreImagen;
        private System.Windows.Forms.PictureBox pictureBoxResultados;
    }
}