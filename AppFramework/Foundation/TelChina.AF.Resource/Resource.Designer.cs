namespace TelChina.AF.Resource
{
    partial class Resource
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.resourceTree = new System.Windows.Forms.TreeView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.ResourceGrid = new System.Windows.Forms.DataGridView();
            this.ResourceCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResourceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.OrderNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ResourceGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // resourceTree
            // 
            this.resourceTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.resourceTree.Location = new System.Drawing.Point(2, 1);
            this.resourceTree.Name = "resourceTree";
            this.resourceTree.Size = new System.Drawing.Size(149, 394);
            this.resourceTree.TabIndex = 0;
            this.resourceTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.resourceTree_AfterSelect);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(488, 401);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "保存";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(630, 401);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ResourceGrid
            // 
            this.ResourceGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResourceGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ResourceGrid.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.ResourceGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ResourceCode,
            this.ResourceName,
            this.IsVisible,
            this.OrderNo});
            this.ResourceGrid.Location = new System.Drawing.Point(169, 1);
            this.ResourceGrid.Name = "ResourceGrid";
            this.ResourceGrid.RowTemplate.Height = 23;
            this.ResourceGrid.Size = new System.Drawing.Size(676, 394);
            this.ResourceGrid.TabIndex = 4;
            this.ResourceGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnValueChanged);
            // 
            // ResourceCode
            // 
            this.ResourceCode.DataPropertyName = "ResourceCode";
            this.ResourceCode.HeaderText = "资源编码";
            this.ResourceCode.Name = "ResourceCode";
            this.ResourceCode.ReadOnly = true;
            this.ResourceCode.ToolTipText = "ResourceCode";
            // 
            // ResourceName
            // 
            this.ResourceName.DataPropertyName = "ResourceName";
            this.ResourceName.FillWeight = 300F;
            this.ResourceName.HeaderText = "资源描述";
            this.ResourceName.Name = "ResourceName";
            this.ResourceName.ToolTipText = "ResourceName";
            // 
            // IsVisible
            // 
            this.IsVisible.DataPropertyName = "IsVisible";
            this.IsVisible.HeaderText = "是否可见";
            this.IsVisible.Name = "IsVisible";
            this.IsVisible.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsVisible.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsVisible.ToolTipText = "IsVisible";
            // 
            // OrderNo
            // 
            this.OrderNo.DataPropertyName = "OrderNo";
            this.OrderNo.HeaderText = "排序";
            this.OrderNo.Name = "OrderNo";
            this.OrderNo.ToolTipText = "OrderNo";
            // 
            // Resource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 430);
            this.Controls.Add(this.ResourceGrid);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.resourceTree);
            this.Name = "Resource";
            this.Text = "资源编辑";
            ((System.ComponentModel.ISupportInitialize)(this.ResourceGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView resourceTree;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView ResourceGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResourceCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResourceName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNo;

    }
}

