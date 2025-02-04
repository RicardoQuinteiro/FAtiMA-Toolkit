﻿using System;
using System.Linq;
using CommeillFaut;
using CommeillFaut.DTOs;
using Equin.ApplicationFramework;
using System.Collections;
using GAIPS.AssetEditorTools;
using System.Windows.Forms;

namespace CommeillFautWF
{
    public partial class MainForm : BaseCIFForm
    {
        private ConditionSetView conditions;
        private BindingListView<SocialExchangeDTO> _socialExchangeList; 

        public MainForm()
        {
            InitializeComponent();
          
        }

        protected override void OnAssetDataLoaded(CommeillFautAsset asset)
        {
           this._socialExchangeList = new BindingListView<SocialExchangeDTO>((IList)null);
			gridSocialExchanges.DataSource = this._socialExchangeList;

            conditions = new ConditionSetView();
            conditionSetEditorControl.View = conditions;
            conditions.OnDataChanged += ConditionSetView_OnDataChanged;


            this._socialExchangeList.DataSource = LoadedAsset.GetAllSocialExchanges().ToList();
            
            EditorTools.HideColumns(gridSocialExchanges, new[] {
            PropertyUtil.GetPropertyName<SocialExchangeDTO>(dto => dto.Id),
            PropertyUtil.GetPropertyName<SocialExchangeDTO>(dto => dto.StartingConditions)});

            if (this._socialExchangeList.Any())
			{
				var ra = LoadedAsset.GetSocialExchange(this._socialExchangeList.First().Id);
				UpdateConditions(ra);
			}
        }

        private void ConditionSetView_OnDataChanged()
        {
            var selectedRule = EditorTools.GetSelectedDtoFromTable<SocialExchangeDTO>(gridSocialExchanges);
            if (selectedRule == null)
                return;
            selectedRule.StartingConditions = conditions.GetData();
            LoadedAsset.AddOrUpdateExchange(selectedRule);

            SetModified();
        }

        private void buttonAddSE_Click(object sender, EventArgs e)
        {
            var dto = new SocialExchangeDTO()
            {
                Description = "-",
                Name = WellFormedNames.Name.BuildName("SE1"),
                Target = WellFormedNames.Name.BuildName("[t]"),
                Id = new Guid(),
                StartingConditions = new Conditions.DTOs.ConditionSetDTO(),
                Steps = new System.Collections.Generic.List<WellFormedNames.Name>(),
                InfluenceRules = new System.Collections.Generic.List<InfluenceRuleDTO>()
            
            };

           this.auxAddOrUpdateItem(dto);
        }

        private void buttonEditSE_Click(object sender, EventArgs e)
        {
            var rule = EditorTools.GetSelectedDtoFromTable<SocialExchangeDTO>(this.gridSocialExchanges);
            if (rule != null)
            {
                this.auxAddOrUpdateItem(rule);
            }
        }
        private void auxAddOrUpdateItem(SocialExchangeDTO item)
        {
            var diag = new AddSocialExchange(LoadedAsset, item);
            diag.ShowDialog(this);
            if (diag.UpdatedGuid != Guid.Empty)
            {
              //  _socialExchangeList.DataSource = LoadedAsset.GetAllSocialExchanges().ToList();

                    this._socialExchangeList.DataSource = LoadedAsset.GetAllSocialExchanges().ToList();
            


                EditorTools.HighlightItemInGrid<SocialExchangeDTO>
                    (gridSocialExchanges, diag.UpdatedGuid);
            }

            SetModified();
        }


        private void gridSocialExchanges_SelectionChanged(object sender, EventArgs e)
        {
            var se = EditorTools.GetSelectedDtoFromTable<SocialExchangeDTO>(this.gridSocialExchanges);
            if (se != null) conditions.SetData(se.StartingConditions);
            else conditions.SetData(null);
        }

        private void buttonDuplicateSE_Click(object sender, EventArgs e)
        {
            var r = EditorTools.GetSelectedDtoFromTable<SocialExchangeDTO>(
                this.gridSocialExchanges);

            if (r != null)
            {
                r.Id = Guid.Empty;
                var newRuleId = LoadedAsset.AddOrUpdateExchange(r);
                _socialExchangeList.DataSource = LoadedAsset.GetAllSocialExchanges().ToList();
                EditorTools.HighlightItemInGrid<SocialExchangeDTO>(gridSocialExchanges, newRuleId);
            }
        }

        private void buttonRemoveSE_Click(object sender, EventArgs e)
        {
            var selRows = gridSocialExchanges.SelectedRows;
            if (selRows.Count == 0) return;
            foreach (var r in selRows.Cast<DataGridViewRow>())
            {
                var dto = ((ObjectView<SocialExchangeDTO>)r.DataBoundItem).Object;
                LoadedAsset.RemoveSocialExchange(dto.Id);
            }
            _socialExchangeList.DataSource = LoadedAsset.GetAllSocialExchanges().ToList();
            EditorTools.HighlightItemInGrid<SocialExchangeDTO>(gridSocialExchanges, Guid.Empty);
            SetModified();
        }

        private void gridSocialExchanges_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1) //exclude header cells
            {
                buttonEditSE_Click(sender, e);
            }
        }

        private void gridSocialExchanges_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.buttonEditSE_Click(sender, e);
                    e.Handled = true;
                    break;
                case Keys.D:
                    if (e.Control) this.buttonDuplicateSE_Click(sender, e);
                    break;
                case Keys.Delete:
                    this.buttonRemoveSE_Click(sender, e);
                    break;
            }
        }

        	private void UpdateConditions(SocialExchangeDTO reaction)
		{
			conditions.SetData(reaction?.StartingConditions);
		}

        private void influenceRules_Click(object sender, EventArgs e)
        {
             var selectedRule = EditorTools.GetSelectedDtoFromTable<SocialExchangeDTO>(gridSocialExchanges);
            var newSE = new SocialExchange(selectedRule);
            var diag = new InfluenceRuleInspector(LoadedAsset, newSE);
            diag.ShowDialog(this);
            LoadedAsset.UpdateSocialExchange(newSE.ToDTO);
             _socialExchangeList.DataSource = LoadedAsset.GetAllSocialExchanges().ToList();
            SetModified();
        }

        private void gridSocialExchanges_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }
    }
}

    


      