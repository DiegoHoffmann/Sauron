namespace Sauron
{
    partial class Captura
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
            this.btnBuscarPessoa = new System.Windows.Forms.Button();
            this.player1 = new Accord.Controls.VideoSourcePlayer();
            this.player2 = new Accord.Controls.VideoSourcePlayer();
            this.player3 = new Accord.Controls.VideoSourcePlayer();
            this.player4 = new Accord.Controls.VideoSourcePlayer();
            this.btnCadastroFaces = new System.Windows.Forms.Button();
            this.btnConfigurar = new System.Windows.Forms.Button();
            this.btnConectar = new System.Windows.Forms.Button();
            this.btnExcluirFaces = new System.Windows.Forms.Button();
            this.btnTreinarReconhecimento = new System.Windows.Forms.Button();
            this.lvFaces = new System.Windows.Forms.ListView();
            this.cbHistorico = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnBuscarPessoa
            // 
            this.btnBuscarPessoa.Location = new System.Drawing.Point(828, 12);
            this.btnBuscarPessoa.Name = "btnBuscarPessoa";
            this.btnBuscarPessoa.Size = new System.Drawing.Size(138, 23);
            this.btnBuscarPessoa.TabIndex = 1;
            this.btnBuscarPessoa.Text = "Iniciar Busca";
            this.btnBuscarPessoa.UseVisualStyleBackColor = true;
            this.btnBuscarPessoa.Click += new System.EventHandler(this.btnBuscarPessoa_Click);
            // 
            // player1
            // 
            this.player1.Location = new System.Drawing.Point(12, 12);
            this.player1.Name = "player1";
            this.player1.Size = new System.Drawing.Size(400, 300);
            this.player1.TabIndex = 2;
            this.player1.Text = "Sauron 1";
            this.player1.VideoSource = null;
            this.player1.Click += new System.EventHandler(this.videoSourcePlayer_Click);
            // 
            // player2
            // 
            this.player2.Location = new System.Drawing.Point(418, 12);
            this.player2.Name = "player2";
            this.player2.Size = new System.Drawing.Size(400, 300);
            this.player2.TabIndex = 3;
            this.player2.Text = "videoSourcePlayer1";
            this.player2.VideoSource = null;
            this.player2.Click += new System.EventHandler(this.player2_Click);
            // 
            // player3
            // 
            this.player3.Location = new System.Drawing.Point(12, 318);
            this.player3.Name = "player3";
            this.player3.Size = new System.Drawing.Size(400, 300);
            this.player3.TabIndex = 4;
            this.player3.Text = "videoSourcePlayer2";
            this.player3.VideoSource = null;
            this.player3.Click += new System.EventHandler(this.player3_Click);
            // 
            // player4
            // 
            this.player4.Location = new System.Drawing.Point(418, 318);
            this.player4.Name = "player4";
            this.player4.Size = new System.Drawing.Size(400, 300);
            this.player4.TabIndex = 5;
            this.player4.Text = "videoSourcePlayer1";
            this.player4.VideoSource = null;
            this.player4.Click += new System.EventHandler(this.player4_Click);
            // 
            // btnCadastroFaces
            // 
            this.btnCadastroFaces.Location = new System.Drawing.Point(828, 41);
            this.btnCadastroFaces.Name = "btnCadastroFaces";
            this.btnCadastroFaces.Size = new System.Drawing.Size(138, 22);
            this.btnCadastroFaces.TabIndex = 6;
            this.btnCadastroFaces.Text = "Cadastro Faces";
            this.btnCadastroFaces.UseVisualStyleBackColor = true;
            this.btnCadastroFaces.Click += new System.EventHandler(this.btnCadastroFaces_Click);
            // 
            // btnConfigurar
            // 
            this.btnConfigurar.Location = new System.Drawing.Point(828, 596);
            this.btnConfigurar.Name = "btnConfigurar";
            this.btnConfigurar.Size = new System.Drawing.Size(138, 22);
            this.btnConfigurar.TabIndex = 7;
            this.btnConfigurar.Text = "Configurar";
            this.btnConfigurar.UseVisualStyleBackColor = true;
            this.btnConfigurar.Click += new System.EventHandler(this.btnConfigurar_Click);
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(828, 568);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(138, 22);
            this.btnConectar.TabIndex = 8;
            this.btnConectar.Text = "Conectar Câmeras";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // btnExcluirFaces
            // 
            this.btnExcluirFaces.Location = new System.Drawing.Point(828, 98);
            this.btnExcluirFaces.Name = "btnExcluirFaces";
            this.btnExcluirFaces.Size = new System.Drawing.Size(138, 22);
            this.btnExcluirFaces.TabIndex = 9;
            this.btnExcluirFaces.Text = "Excluir Faces";
            this.btnExcluirFaces.UseVisualStyleBackColor = true;
            this.btnExcluirFaces.Click += new System.EventHandler(this.btnExcluirFaces_Click);
            // 
            // btnTreinarReconhecimento
            // 
            this.btnTreinarReconhecimento.Location = new System.Drawing.Point(828, 70);
            this.btnTreinarReconhecimento.Name = "btnTreinarReconhecimento";
            this.btnTreinarReconhecimento.Size = new System.Drawing.Size(138, 22);
            this.btnTreinarReconhecimento.TabIndex = 10;
            this.btnTreinarReconhecimento.Text = "Treinar Reconhecimento";
            this.btnTreinarReconhecimento.UseVisualStyleBackColor = true;
            this.btnTreinarReconhecimento.Click += new System.EventHandler(this.btnTreinarReconhecimento_Click);
            // 
            // lvFaces
            // 
            this.lvFaces.HideSelection = false;
            this.lvFaces.Location = new System.Drawing.Point(828, 149);
            this.lvFaces.Name = "lvFaces";
            this.lvFaces.Size = new System.Drawing.Size(138, 413);
            this.lvFaces.TabIndex = 11;
            this.lvFaces.UseCompatibleStateImageBehavior = false;
            this.lvFaces.View = System.Windows.Forms.View.List;
            // 
            // cbHistorico
            // 
            this.cbHistorico.AutoSize = true;
            this.cbHistorico.Location = new System.Drawing.Point(828, 126);
            this.cbHistorico.Name = "cbHistorico";
            this.cbHistorico.Size = new System.Drawing.Size(103, 17);
            this.cbHistorico.TabIndex = 12;
            this.cbHistorico.Text = "Mostrar histórico";
            this.cbHistorico.UseVisualStyleBackColor = true;
            // 
            // Captura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 630);
            this.Controls.Add(this.cbHistorico);
            this.Controls.Add(this.lvFaces);
            this.Controls.Add(this.btnTreinarReconhecimento);
            this.Controls.Add(this.btnExcluirFaces);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.btnConfigurar);
            this.Controls.Add(this.btnCadastroFaces);
            this.Controls.Add(this.player4);
            this.Controls.Add(this.player3);
            this.Controls.Add(this.player2);
            this.Controls.Add(this.player1);
            this.Controls.Add(this.btnBuscarPessoa);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Captura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sauron";
            this.Activated += new System.EventHandler(this.reiniciar);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FecharExecucao);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
        #endregion
        private System.Windows.Forms.Button btnBuscarPessoa;
        private Accord.Controls.VideoSourcePlayer player1;
        private Accord.Controls.VideoSourcePlayer player2;
        private Accord.Controls.VideoSourcePlayer player3;
        private Accord.Controls.VideoSourcePlayer player4;
        private System.Windows.Forms.Button btnCadastroFaces;
        private System.Windows.Forms.Button btnConfigurar;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.Button btnExcluirFaces;
        private System.Windows.Forms.Button btnTreinarReconhecimento;
        private System.Windows.Forms.ListView lvFaces;
        private System.Windows.Forms.CheckBox cbHistorico;
    }
}

