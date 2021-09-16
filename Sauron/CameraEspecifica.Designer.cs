namespace Sauron
{
    partial class CameraEspecifica
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
            this.playerEspecifico = new Accord.Controls.VideoSourcePlayer();
            this.btnCapturarImagem = new System.Windows.Forms.Button();
            this.btnIniciarBusca = new System.Windows.Forms.Button();
            this.btnBuscaApi = new System.Windows.Forms.Button();
            this.lvFaces = new System.Windows.Forms.ListView();
            this.cbHistorico = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // playerEspecifico
            // 
            this.playerEspecifico.Location = new System.Drawing.Point(12, 12);
            this.playerEspecifico.Name = "playerEspecifico";
            this.playerEspecifico.Size = new System.Drawing.Size(800, 600);
            this.playerEspecifico.TabIndex = 0;
            this.playerEspecifico.Text = "videoSourcePlayer1";
            this.playerEspecifico.VideoSource = null;
            // 
            // btnCapturarImagem
            // 
            this.btnCapturarImagem.Location = new System.Drawing.Point(835, 14);
            this.btnCapturarImagem.Name = "btnCapturarImagem";
            this.btnCapturarImagem.Size = new System.Drawing.Size(128, 33);
            this.btnCapturarImagem.TabIndex = 1;
            this.btnCapturarImagem.Text = "Capturar Imagem";
            this.btnCapturarImagem.UseVisualStyleBackColor = true;
            this.btnCapturarImagem.Click += new System.EventHandler(this.btnCapturarImagem_Click);
            // 
            // btnIniciarBusca
            // 
            this.btnIniciarBusca.Location = new System.Drawing.Point(837, 53);
            this.btnIniciarBusca.Name = "btnIniciarBusca";
            this.btnIniciarBusca.Size = new System.Drawing.Size(126, 33);
            this.btnIniciarBusca.TabIndex = 2;
            this.btnIniciarBusca.Text = "Buscar Pessoa";
            this.btnIniciarBusca.UseVisualStyleBackColor = true;
            this.btnIniciarBusca.Click += new System.EventHandler(this.btnIniciarBusca_click);
            // 
            // btnBuscaApi
            // 
            this.btnBuscaApi.Location = new System.Drawing.Point(836, 104);
            this.btnBuscaApi.Name = "btnBuscaApi";
            this.btnBuscaApi.Size = new System.Drawing.Size(126, 18);
            this.btnBuscaApi.TabIndex = 3;
            this.btnBuscaApi.Text = "Busca por API";
            this.btnBuscaApi.UseVisualStyleBackColor = true;
            this.btnBuscaApi.Visible = false;
            this.btnBuscaApi.Click += new System.EventHandler(this.btnBuscaApi_Click);
            // 
            // lvFaces
            // 
            this.lvFaces.HideSelection = false;
            this.lvFaces.Location = new System.Drawing.Point(818, 151);
            this.lvFaces.Name = "lvFaces";
            this.lvFaces.Size = new System.Drawing.Size(159, 461);
            this.lvFaces.TabIndex = 4;
            this.lvFaces.UseCompatibleStateImageBehavior = false;
            this.lvFaces.View = System.Windows.Forms.View.List;
            // 
            // cbHistorico
            // 
            this.cbHistorico.AutoSize = true;
            this.cbHistorico.Location = new System.Drawing.Point(818, 128);
            this.cbHistorico.Name = "cbHistorico";
            this.cbHistorico.Size = new System.Drawing.Size(103, 17);
            this.cbHistorico.TabIndex = 5;
            this.cbHistorico.Text = "Mostrar histórico";
            this.cbHistorico.UseVisualStyleBackColor = true;
            // 
            // CameraEspecifica
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(984, 625);
            this.Controls.Add(this.cbHistorico);
            this.Controls.Add(this.lvFaces);
            this.Controls.Add(this.btnBuscaApi);
            this.Controls.Add(this.btnIniciarBusca);
            this.Controls.Add(this.btnCapturarImagem);
            this.Controls.Add(this.playerEspecifico);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CameraEspecifica";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sauron";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FecharExecucao);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Accord.Controls.VideoSourcePlayer playerEspecifico;
        private System.Windows.Forms.Button btnCapturarImagem;
        private System.Windows.Forms.Button btnIniciarBusca;
        private System.Windows.Forms.Button btnBuscaApi;
        private System.Windows.Forms.ListView lvFaces;
        private System.Windows.Forms.CheckBox cbHistorico;
    }
}