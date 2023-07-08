using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using STGeneticsWebApp.Data;

namespace STGeneticsWebApp.Pages
{
    public partial class AnimalGrid
    {
        [Inject]
        STGeneticsService STGeneticsService { get; set; }
        List<AnimalViewModel> AnimalList { get; set; }
        List<Breed> BreedList { get; set; }
        List<Sex> SexList { get; set; }
        List<Status> StatusList { get; set; }

        DxGrid Grid;

        protected override async Task OnInitializedAsync()
        {
            AnimalList = await STGeneticsService.GetAnimalListAsync();
            BreedList = await STGeneticsService.GetBreedListAsync();
            SexList = await STGeneticsService.GetSexListAsync();
            StatusList = await STGeneticsService.GetStatusListAsync();
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            if (e.IsNew)
            {
                _ = await STGeneticsService.InsertAnimal((e.EditModel as AnimalViewModel).ToAnimal());
            }
            else
            {
                await STGeneticsService.UpdateAnimal((e.EditModel as AnimalViewModel).ToAnimal());
            }

            e.Grid.ClearSelection();

            this.AnimalList = await STGeneticsService.GetAnimalListAsync();
        }

        void OnEditCanceling(GridEditCancelingEventArgs e)
        {
            e.Grid.ClearSelection();
        }

        void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            if (e.IsNew)
            {
                var newTravelItinerary = (AnimalViewModel)e.EditModel;

                newTravelItinerary.BirthDate = System.DateTime.Today;
            }
        }

        async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            await STGeneticsService.DeleteAnimal((e.DataItem as AnimalViewModel).ToAnimal());
            this.AnimalList = await STGeneticsService.GetAnimalListAsync();
        }

        void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow)
            {
                if (e.VisibleIndex % 2 == 1)
                    e.CssClass = "alt-row";
                else
                    e.CssClass = "hover-row";
            }

            if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.1)";
                e.CssClass = "header-bold";
            }
        }

        void Grid_CustomizeSummaryDisplayText(GridCustomizeSummaryDisplayTextEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "AnimalCountSummary":
                    e.DisplayText = string.Format("{0:N0} Animals", e.Value);
                    break;
                case "AnimalPriceSummary":
                    e.DisplayText = string.Format("{0:C0}", e.Value);
                    break;
                default:
                    break;
            }
        }

        void OnColumnChooserClick()
        {
            Grid.ShowColumnChooser(".stgenetics-column-chooser");
        }

        async Task ExportXlsx_Click()
        {
            await Grid.ExportToXlsxAsync("Animals " + System.DateTime.Now.ToString("ddMMMyyyy-hhmmsstt"), new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = false
            });
        }
    }
}
