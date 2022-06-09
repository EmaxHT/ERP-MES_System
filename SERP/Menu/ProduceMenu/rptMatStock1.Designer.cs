namespace SERP
{
    partial class rptMatStock1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rptMatStock1));
            this.gc_Stock = new SERP.GridControlEx();
            this.gv_Stock = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel_H = new DevExpress.XtraEditors.PanelControl();
            this.txt_WhouseName = new DevExpress.XtraEditors.TextEdit();
            this.txt_WhouseCode = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panReg)).BeginInit();
            this.panReg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panButton)).BeginInit();
            this.panButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Stock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Stock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_H)).BeginInit();
            this.panel_H.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_WhouseName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_WhouseCode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Save
            // 
            this.btn_Save.Appearance.BackColor = System.Drawing.Color.White;
            this.btn_Save.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(85)))), ((int)(((byte)(117)))));
            this.btn_Save.Appearance.Options.UseBackColor = true;
            this.btn_Save.Appearance.Options.UseBorderColor = true;
            this.btn_Save.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Save.ImageOptions.SvgImage")));
            // 
            // btn_Close
            // 
            this.btn_Close.Appearance.BackColor = System.Drawing.Color.White;
            this.btn_Close.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Close.Appearance.Options.UseBackColor = true;
            this.btn_Close.Appearance.Options.UseBorderColor = true;
            this.btn_Close.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Close.ImageOptions.Image")));
            this.btn_Close.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Close.ImageOptions.SvgImage")));
            // 
            // btn_Excel
            // 
            this.btn_Excel.Appearance.BackColor = System.Drawing.Color.White;
            this.btn_Excel.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Excel.Appearance.Options.UseBackColor = true;
            this.btn_Excel.Appearance.Options.UseBorderColor = true;
            this.btn_Excel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Excel.ImageOptions.Image")));
            this.btn_Excel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Excel.ImageOptions.SvgImage")));
            // 
            // btn_Insert
            // 
            this.btn_Insert.Appearance.BackColor = System.Drawing.Color.White;
            this.btn_Insert.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Insert.Appearance.Options.UseBackColor = true;
            this.btn_Insert.Appearance.Options.UseBorderColor = true;
            this.btn_Insert.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Insert.ImageOptions.SvgImage")));
            // 
            // btn_Delete
            // 
            this.btn_Delete.Appearance.BackColor = System.Drawing.Color.White;
            this.btn_Delete.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Delete.Appearance.Options.UseBackColor = true;
            this.btn_Delete.Appearance.Options.UseBorderColor = true;
            this.btn_Delete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Delete.ImageOptions.SvgImage")));
            // 
            // btn_Select
            // 
            this.btn_Select.Appearance.BackColor = System.Drawing.Color.White;
            this.btn_Select.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Select.Appearance.Options.UseBackColor = true;
            this.btn_Select.Appearance.Options.UseBorderColor = true;
            this.btn_Select.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Select.ImageOptions.SvgImage")));
            // 
            // btn_Print
            // 
            this.btn_Print.Appearance.BackColor = System.Drawing.Color.White;
            this.btn_Print.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Print.Appearance.Options.UseBackColor = true;
            this.btn_Print.Appearance.Options.UseBorderColor = true;
            this.btn_Print.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Print.ImageOptions.SvgImage")));
            // 
            // panReg
            // 
            this.panReg.Size = new System.Drawing.Size(1500, 40);
            // 
            // panButton
            // 
            this.panButton.Location = new System.Drawing.Point(945, 2);
            // 
            // gc_Stock
            // 
            this.gc_Stock.AddRowYN = false;
            this.gc_Stock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc_Stock.EnterYN = true;
            this.gc_Stock.Execl_GB = SERP.GridControlEx.Excel_GB.Update;
            this.gc_Stock.ExpansionCHK = false;
            this.gc_Stock.Location = new System.Drawing.Point(0, 101);
            this.gc_Stock.MainView = this.gv_Stock;
            this.gc_Stock.MouseWheelChk = true;
            this.gc_Stock.MultiSelectChk = true;
            this.gc_Stock.Name = "gc_Stock";
            this.gc_Stock.PopMenuChk = true;
            this.gc_Stock.Size = new System.Drawing.Size(1500, 493);
            this.gc_Stock.TabIndex = 13;
            this.gc_Stock.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_Stock});
            // 
            // gv_Stock
            // 
            this.gv_Stock.Appearance.FocusedRow.BackColor = System.Drawing.Color.Transparent;
            this.gv_Stock.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gv_Stock.GridControl = this.gc_Stock;
            this.gv_Stock.Name = "gv_Stock";
            this.gv_Stock.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.Update;
            this.gv_Stock.OptionsSelection.MultiSelect = true;
            this.gv_Stock.OptionsView.ShowGroupPanel = false;
            // 
            // panel_H
            // 
            this.panel_H.Controls.Add(this.txt_WhouseName);
            this.panel_H.Controls.Add(this.txt_WhouseCode);
            this.panel_H.Controls.Add(this.labelControl5);
            this.panel_H.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_H.Location = new System.Drawing.Point(0, 40);
            this.panel_H.Name = "panel_H";
            this.panel_H.Size = new System.Drawing.Size(1500, 61);
            this.panel_H.TabIndex = 12;
            // 
            // txt_WhouseName
            // 
            this.txt_WhouseName.Enabled = false;
            this.txt_WhouseName.Location = new System.Drawing.Point(199, 14);
            this.txt_WhouseName.Name = "txt_WhouseName";
            this.txt_WhouseName.Properties.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.txt_WhouseName.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_WhouseName.Properties.Appearance.Options.UseBackColor = true;
            this.txt_WhouseName.Properties.Appearance.Options.UseFont = true;
            this.txt_WhouseName.Properties.AutoHeight = false;
            this.txt_WhouseName.Size = new System.Drawing.Size(100, 32);
            this.txt_WhouseName.TabIndex = 49;
            this.txt_WhouseName.EditValueChanged += new System.EventHandler(this.txt_WhouseName_EditValueChanged);
            // 
            // txt_WhouseCode
            // 
            this.txt_WhouseCode.Location = new System.Drawing.Point(93, 14);
            this.txt_WhouseCode.Name = "txt_WhouseCode";
            this.txt_WhouseCode.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(247)))), ((int)(((byte)(182)))));
            this.txt_WhouseCode.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_WhouseCode.Properties.Appearance.Options.UseBackColor = true;
            this.txt_WhouseCode.Properties.Appearance.Options.UseFont = true;
            this.txt_WhouseCode.Properties.AutoHeight = false;
            this.txt_WhouseCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search)});
            this.txt_WhouseCode.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txt_WhouseCode_Properties_ButtonClick);
            this.txt_WhouseCode.Size = new System.Drawing.Size(100, 32);
            this.txt_WhouseCode.TabIndex = 1;
            this.txt_WhouseCode.EditValueChanged += new System.EventHandler(this.txt_WhouseCode_EditValueChanged);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(23, 20);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 19);
            this.labelControl5.TabIndex = 0;
            this.labelControl5.Text = "창고코드";
            // 
            // rptMatStock1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gc_Stock);
            this.Controls.Add(this.panel_H);
            this.Name = "rptMatStock1";
            this.Size = new System.Drawing.Size(1500, 594);
            this.Load += new System.EventHandler(this.rptMatStock1_Load);
            this.Controls.SetChildIndex(this.panReg, 0);
            this.Controls.SetChildIndex(this.panel_H, 0);
            this.Controls.SetChildIndex(this.gc_Stock, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panReg)).EndInit();
            this.panReg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panButton)).EndInit();
            this.panButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gc_Stock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Stock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_H)).EndInit();
            this.panel_H.ResumeLayout(false);
            this.panel_H.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_WhouseName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_WhouseCode.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GridControlEx gc_Stock;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_Stock;
        private DevExpress.XtraEditors.PanelControl panel_H;
        private DevExpress.XtraEditors.TextEdit txt_WhouseName;
        private DevExpress.XtraEditors.ButtonEdit txt_WhouseCode;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}
