namespace Servicios
{
    partial class GestionFamilia
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
            this.dgvFamilias = new System.Windows.Forms.DataGridView();
            this.dgvPermisosFamilias = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tvPermisosAsignados = new System.Windows.Forms.TreeView();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCrear = new System.Windows.Forms.Button();
            this.btnAsignar = new System.Windows.Forms.Button();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFamilias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPermisosFamilias)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvFamilias
            // 
            this.dgvFamilias.BackgroundColor = System.Drawing.Color.White;
            this.dgvFamilias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFamilias.Location = new System.Drawing.Point(95, 95);
            this.dgvFamilias.Name = "dgvFamilias";
            this.dgvFamilias.RowHeadersWidth = 82;
            this.dgvFamilias.RowTemplate.Height = 33;
            this.dgvFamilias.Size = new System.Drawing.Size(554, 742);
            this.dgvFamilias.TabIndex = 0;
            // 
            // dgvPermisosFamilias
            // 
            this.dgvPermisosFamilias.BackgroundColor = System.Drawing.Color.White;
            this.dgvPermisosFamilias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPermisosFamilias.Location = new System.Drawing.Point(1443, 95);
            this.dgvPermisosFamilias.Name = "dgvPermisosFamilias";
            this.dgvPermisosFamilias.RowHeadersWidth = 82;
            this.dgvPermisosFamilias.RowTemplate.Height = 33;
            this.dgvPermisosFamilias.Size = new System.Drawing.Size(554, 742);
            this.dgvPermisosFamilias.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(271, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 38);
            this.label1.TabIndex = 2;
            this.label1.Text = "Familias";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1531, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(375, 38);
            this.label2.TabIndex = 3;
            this.label2.Text = "Permisos / Familias";
            // 
            // tvPermisosAsignados
            // 
            this.tvPermisosAsignados.Location = new System.Drawing.Point(769, 95);
            this.tvPermisosAsignados.Name = "tvPermisosAsignados";
            this.tvPermisosAsignados.Size = new System.Drawing.Size(554, 742);
            this.tvPermisosAsignados.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(941, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 38);
            this.label3.TabIndex = 5;
            this.label3.Text = "Asignados";
            // 
            // btnCrear
            // 
            this.btnCrear.BackColor = System.Drawing.Color.SteelBlue;
            this.btnCrear.Font = new System.Drawing.Font("Verdana", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrear.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCrear.Location = new System.Drawing.Point(207, 884);
            this.btnCrear.Margin = new System.Windows.Forms.Padding(4);
            this.btnCrear.Name = "btnCrear";
            this.btnCrear.Size = new System.Drawing.Size(340, 133);
            this.btnCrear.TabIndex = 16;
            this.btnCrear.Text = "CREAR";
            this.btnCrear.UseVisualStyleBackColor = false;
            // 
            // btnAsignar
            // 
            this.btnAsignar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAsignar.Font = new System.Drawing.Font("Verdana", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAsignar.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAsignar.Location = new System.Drawing.Point(885, 884);
            this.btnAsignar.Margin = new System.Windows.Forms.Padding(4);
            this.btnAsignar.Name = "btnAsignar";
            this.btnAsignar.Size = new System.Drawing.Size(340, 133);
            this.btnAsignar.TabIndex = 17;
            this.btnAsignar.Text = "ASIGNAR";
            this.btnAsignar.UseVisualStyleBackColor = false;
            // 
            // btnAplicar
            // 
            this.btnAplicar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAplicar.Font = new System.Drawing.Font("Verdana", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAplicar.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAplicar.Location = new System.Drawing.Point(1557, 884);
            this.btnAplicar.Margin = new System.Windows.Forms.Padding(4);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(340, 133);
            this.btnAplicar.TabIndex = 18;
            this.btnAplicar.Text = "APLICAR";
            this.btnAplicar.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtNombre);
            this.groupBox1.Enabled = false;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(769, 1078);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 164);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(12, 85);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(536, 46);
            this.txtNombre.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 57);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 25);
            this.label4.TabIndex = 20;
            this.label4.Text = "Nombre";
            // 
            // GestionFamilia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(2095, 1267);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.btnAsignar);
            this.Controls.Add(this.btnCrear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tvPermisosAsignados);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvPermisosFamilias);
            this.Controls.Add(this.dgvFamilias);
            this.Name = "GestionFamilia";
            this.Text = "GestionFamilia";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFamilias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPermisosFamilias)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFamilias;
        private System.Windows.Forms.DataGridView dgvPermisosFamilias;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView tvPermisosAsignados;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCrear;
        private System.Windows.Forms.Button btnAsignar;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label4;
    }
}