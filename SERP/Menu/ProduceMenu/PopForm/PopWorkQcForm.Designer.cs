namespace SERP
{
    partial class PopWorkQcForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PopWorkQcForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.label_Name = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Save = new SERP.SimpleButtonEx();
            this.btn_Close = new SERP.SimpleButtonEx();
            this.panel_H = new DevExpress.XtraEditors.PanelControl();
            this.txt_Customer1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Qty = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txt_ItemName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txt_ItemCode = new DevExpress.XtraEditors.TextEdit();
            this.labelControl28 = new DevExpress.XtraEditors.LabelControl();
            this.txt_ResultNo = new DevExpress.XtraEditors.TextEdit();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gc_QcS1 = new SERP.GridControlEx();
            this.gv_QcS1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panel_H)).BeginInit();
            this.panel_H.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Customer1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Qty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ItemName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ItemCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ResultNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gc_QcS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_QcS1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.label_Name);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(544, 51);
            this.panelControl1.TabIndex = 2;
            // 
            // label_Name
            // 
            this.label_Name.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Name.Appearance.Options.UseFont = true;
            this.label_Name.Location = new System.Drawing.Point(18, 13);
            this.label_Name.Name = "label_Name";
            this.label_Name.Size = new System.Drawing.Size(72, 23);
            this.label_Name.TabIndex = 10;
            this.label_Name.Text = "공정검사";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btn_Save);
            this.panelControl2.Controls.Add(this.btn_Close);
            this.panelControl2.Location = new System.Drawing.Point(375, 5);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(165, 43);
            this.panelControl2.TabIndex = 9;
            // 
            // btn_Save
            // 
            this.btn_Save.Appearance.BackColor = System.Drawing.Color.White;
            this.btn_Save.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Save.Appearance.Options.UseBackColor = true;
            this.btn_Save.Appearance.Options.UseBorderColor = true;
            this.btn_Save.button_GB = SERP.SimpleButtonEx.Button_GB.Save;
            this.btn_Save.ImageOptions.Image = global::SERP.Properties.Resources.cancel_32x32;
            this.btn_Save.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Save.ImageOptions.SvgImage")));
            this.btn_Save.Location = new System.Drawing.Point(5, 4);
            this.btn_Save.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Result_Update = System.Windows.Forms.DialogResult.None;
            this.btn_Save.sCHK = "N";
            this.btn_Save.Size = new System.Drawing.Size(74, 34);
            this.btn_Save.sUpdate = "N";
            this.btn_Save.TabIndex = 16;
            this.btn_Save.Text = "저장";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Appearance.BackColor = System.Drawing.Color.White;
            this.btn_Close.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Close.Appearance.Options.UseBackColor = true;
            this.btn_Close.Appearance.Options.UseBorderColor = true;
            this.btn_Close.button_GB = SERP.SimpleButtonEx.Button_GB.Exit;
            this.btn_Close.ImageOptions.Image = global::SERP.Properties.Resources.cancel_32x32;
            this.btn_Close.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Close.ImageOptions.SvgImage")));
            this.btn_Close.Location = new System.Drawing.Point(85, 4);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Result_Update = System.Windows.Forms.DialogResult.None;
            this.btn_Close.sCHK = "N";
            this.btn_Close.Size = new System.Drawing.Size(74, 34);
            this.btn_Close.sUpdate = "N";
            this.btn_Close.TabIndex = 16;
            this.btn_Close.Text = "닫기";
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // panel_H
            // 
            this.panel_H.Controls.Add(this.txt_Customer1);
            this.panel_H.Controls.Add(this.labelControl3);
            this.panel_H.Controls.Add(this.txt_Qty);
            this.panel_H.Controls.Add(this.labelControl2);
            this.panel_H.Controls.Add(this.txt_ItemName);
            this.panel_H.Controls.Add(this.labelControl1);
            this.panel_H.Controls.Add(this.txt_ItemCode);
            this.panel_H.Controls.Add(this.labelControl28);
            this.panel_H.Controls.Add(this.txt_ResultNo);
            this.panel_H.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_H.Location = new System.Drawing.Point(0, 51);
            this.panel_H.Name = "panel_H";
            this.panel_H.Size = new System.Drawing.Size(544, 102);
            this.panel_H.TabIndex = 0;
            // 
            // txt_Customer1
            // 
            this.txt_Customer1.Enabled = false;
            this.txt_Customer1.Location = new System.Drawing.Point(70, 36);
            this.txt_Customer1.Name = "txt_Customer1";
            this.txt_Customer1.Properties.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.txt_Customer1.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11F);
            this.txt_Customer1.Properties.Appearance.Options.UseBackColor = true;
            this.txt_Customer1.Properties.Appearance.Options.UseFont = true;
            this.txt_Customer1.Size = new System.Drawing.Size(194, 24);
            this.txt_Customer1.TabIndex = 66;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(12, 40);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(39, 17);
            this.labelControl3.TabIndex = 65;
            this.labelControl3.Text = "고객사";
            // 
            // txt_Qty
            // 
            this.txt_Qty.Enabled = false;
            this.txt_Qty.Location = new System.Drawing.Point(328, 36);
            this.txt_Qty.Name = "txt_Qty";
            this.txt_Qty.Properties.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.txt_Qty.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11F);
            this.txt_Qty.Properties.Appearance.Options.UseBackColor = true;
            this.txt_Qty.Properties.Appearance.Options.UseFont = true;
            this.txt_Qty.Size = new System.Drawing.Size(99, 24);
            this.txt_Qty.TabIndex = 64;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(270, 39);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(52, 17);
            this.labelControl2.TabIndex = 63;
            this.labelControl2.Text = "입고수량";
            // 
            // txt_ItemName
            // 
            this.txt_ItemName.Enabled = false;
            this.txt_ItemName.Location = new System.Drawing.Point(266, 66);
            this.txt_ItemName.Name = "txt_ItemName";
            this.txt_ItemName.Properties.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.txt_ItemName.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11F);
            this.txt_ItemName.Properties.Appearance.Options.UseBackColor = true;
            this.txt_ItemName.Properties.Appearance.Options.UseFont = true;
            this.txt_ItemName.Size = new System.Drawing.Size(274, 24);
            this.txt_ItemName.TabIndex = 62;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(12, 69);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 17);
            this.labelControl1.TabIndex = 61;
            this.labelControl1.Text = "품목코드";
            // 
            // txt_ItemCode
            // 
            this.txt_ItemCode.Enabled = false;
            this.txt_ItemCode.Location = new System.Drawing.Point(70, 66);
            this.txt_ItemCode.Name = "txt_ItemCode";
            this.txt_ItemCode.Properties.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.txt_ItemCode.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11F);
            this.txt_ItemCode.Properties.Appearance.Options.UseBackColor = true;
            this.txt_ItemCode.Properties.Appearance.Options.UseFont = true;
            this.txt_ItemCode.Size = new System.Drawing.Size(195, 24);
            this.txt_ItemCode.TabIndex = 60;
            // 
            // labelControl28
            // 
            this.labelControl28.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl28.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.labelControl28.Appearance.Options.UseFont = true;
            this.labelControl28.Appearance.Options.UseForeColor = true;
            this.labelControl28.Location = new System.Drawing.Point(12, 10);
            this.labelControl28.Name = "labelControl28";
            this.labelControl28.Size = new System.Drawing.Size(52, 17);
            this.labelControl28.TabIndex = 57;
            this.labelControl28.Text = "입고번호";
            // 
            // txt_ResultNo
            // 
            this.txt_ResultNo.Enabled = false;
            this.txt_ResultNo.Location = new System.Drawing.Point(70, 7);
            this.txt_ResultNo.Name = "txt_ResultNo";
            this.txt_ResultNo.Properties.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.txt_ResultNo.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11F);
            this.txt_ResultNo.Properties.Appearance.Options.UseBackColor = true;
            this.txt_ResultNo.Properties.Appearance.Options.UseFont = true;
            this.txt_ResultNo.Size = new System.Drawing.Size(194, 24);
            this.txt_ResultNo.TabIndex = 56;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gc_QcS1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 153);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(544, 269);
            this.panelControl3.TabIndex = 1;
            // 
            // gc_QcS1
            // 
            this.gc_QcS1.AddRowYN = false;
            this.gc_QcS1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gc_QcS1.EnterYN = true;
            this.gc_QcS1.ExpansionCHK = true;
            this.gc_QcS1.Location = new System.Drawing.Point(5, 5);
            this.gc_QcS1.MainView = this.gv_QcS1;
            this.gc_QcS1.MouseWheelChk = true;
            this.gc_QcS1.MultiSelectChk = true;
            this.gc_QcS1.Name = "gc_QcS1";
            this.gc_QcS1.PopMenuChk = true;
            this.gc_QcS1.Size = new System.Drawing.Size(534, 259);
            this.gc_QcS1.TabIndex = 17;
            this.gc_QcS1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_QcS1});
            this.gc_QcS1.EditorKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gc_QcS1_EditorKeyPress);
            // 
            // gv_QcS1
            // 
            this.gv_QcS1.GridControl = this.gc_QcS1;
            this.gv_QcS1.Name = "gv_QcS1";
            this.gv_QcS1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.Update;
            this.gv_QcS1.OptionsCustomization.AllowFilter = false;
            this.gv_QcS1.OptionsCustomization.AllowSort = false;
            this.gv_QcS1.OptionsSelection.MultiSelect = true;
            this.gv_QcS1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gv_QcS1.OptionsView.ShowGroupPanel = false;
            this.gv_QcS1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_QcS1_CellValueChanged);
            // 
            // PopWorkQcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 422);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panel_H);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PopWorkQcForm";
            this.Load += new System.EventHandler(this.PopWorkQcForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panel_H)).EndInit();
            this.panel_H.ResumeLayout(false);
            this.panel_H.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Customer1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Qty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ItemName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ItemCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ResultNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gc_QcS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_QcS1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private SimpleButtonEx btn_Close;
        private DevExpress.XtraEditors.PanelControl panel_H;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl label_Name;
        private GridControlEx gc_QcS1;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_QcS1;
        private DevExpress.XtraEditors.LabelControl labelControl28;
        private DevExpress.XtraEditors.TextEdit txt_ResultNo;
        private DevExpress.XtraEditors.TextEdit txt_Qty;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txt_ItemName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_ItemCode;
        private DevExpress.XtraEditors.TextEdit txt_Customer1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private SimpleButtonEx btn_Save;
    }
}
