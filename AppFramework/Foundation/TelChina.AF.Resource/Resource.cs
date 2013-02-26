using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TelChina.AF.Sys.Service;
using System.Collections;
namespace TelChina.AF.Resource
{
    public partial class Resource : Form
    {
        /// <summary>
        /// 资源接口
        /// </summary>
        private IEditResourceService resourceService = ServiceProxy.CreateProxy<IEditResourceService>();

        private ArrayList arrayKeyList = new ArrayList() { "ResourceCode", "ResourceName", "IsVisible", "OrderNo" };
        public Resource()
        {
            InitializeComponent();

            Init();
            
        }

        /// <summary>
        /// 加载树的数据
        /// </summary>
        public void Init()
        {
            this.resourceTree.Enabled = true;
            IList<UserResourceDTO> dtos = resourceService.GetTypeResource();

            foreach (UserResourceDTO dto in dtos)
            {
                TreeNode node = new TreeNode();
                node.Text = dto.ResourceName;
                node.Name = dto.ResourceType;
                node.ToolTipText = dto.ResourceCode;
                this.resourceTree.Nodes.Add(node);
            }

        }

        /// <summary>
        /// 加载Grid数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resourceTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var dtos = resourceService.GetResourceByType(e.Node.Name);

            this.ResourceGrid.DataSource = dtos;
            
            foreach(DataGridViewColumn dataColumn in this.ResourceGrid.Columns)
            {
                if (!arrayKeyList.Contains(dataColumn.Name))
                {
                    dataColumn.Visible = false;
                }
            }
        }

        /// <summary>
        /// 值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                UserResourceDTO userDTO = this.ResourceGrid.Rows[e.RowIndex].DataBoundItem as UserResourceDTO;
                if (userDTO.State == ResourceState.Unchanged)
                {
                    userDTO.State = ResourceState.Updating;
                }
                if (userDTO.State == ResourceState.Inserting)
                {
                    userDTO.State = ResourceState.Inserted;
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                IList<UserResourceDTO> dtos = this.ResourceGrid.DataSource as IList<UserResourceDTO>;

                resourceService.UpdateResource(dtos);
                MessageBox.Show("保存成功！");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "保存失败！");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
