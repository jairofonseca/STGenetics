using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using STGeneticsWebApp.Data;

namespace STGeneticsWebApp.Pages
{
    public partial class AnimalGrid
    {
        [Inject]
        STGeneticsService STGeneticsService { get; set; }
        List<AnimalViewModel> AnimalList { get; set; } = null;
        List<Breed> BreedList { get; set; }
        List<Sex> SexList { get; set; }
        List<Status> StatusList { get; set; }

        IReadOnlyList<object> _selectedDataItems { get; set; }
        IReadOnlyList<object> SelectedDataItems
        {
            get
            {
                return _selectedDataItems;
            }

            set
            {
                _selectedDataItems = value;

                if (value?.Count > 0)
                {
                    PurchaseAmount = value.Sum(animal => (animal as AnimalViewModel).Price);

                    DiscountAmount = Discount5PercentAmount;

                    if (value.Count > 10)
                    {
                        DiscountAmount = Discount5PercentAmount + (PurchaseAmount * (decimal).03);
                    }

                    ShippingAmount = 1000;

                    if (value.Count > 20)
                    {
                        ShippingAmount = 0;
                    }
                }
                else
                {
                    PurchaseAmount = 0;
                    Discount5PercentAmount = 0;
                    DiscountAmount = 0;
                    ShippingAmount = 0;
                }
            }
        }

        DxGrid Grid;
        DxGrid SellsGrid;

        private decimal PurchaseAmount { get; set; }
        private decimal Discount5PercentAmount { get; set; }
        private decimal DiscountAmount { get; set; }
        private decimal ShippingAmount { get; set; }
        private decimal TotalAmount { get { return (PurchaseAmount - DiscountAmount + ShippingAmount); } }
        public string DiscountAppliedText
        {
            get
            {
                var discountAppliedtext = "(0%";

                if (Discount5PercentAmount > 0)
                    discountAppliedtext = "(5%";

                if (SelectedDataItems?.Count > 10)
                    discountAppliedtext += " + 3%";

                discountAppliedtext += ")";

                return (discountAppliedtext);

            }
        }

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
            var gridXlExportOptions = new GridXlExportOptions();

            gridXlExportOptions.ExportSelectedRowsOnly = false;
            gridXlExportOptions.SheetName = "ANIMALS";
            gridXlExportOptions.DocumentOptions.Company = "ST Genetics";
            gridXlExportOptions.DocumentOptions.Author = "Jairo Cesar Fonseca Guerrero";

            await Grid.ExportToXlsxAsync("Animals " + System.DateTime.Now.ToString("ddMMMyyyy-hhmmsstt"), gridXlExportOptions);
        }

        async Task ExportSellXlsx_Click()
        {
            var gridXlExportOptions = new GridXlExportOptions();

            gridXlExportOptions.ExportSelectedRowsOnly = false;
            gridXlExportOptions.SheetName = "ANIMALS SOLD";
            gridXlExportOptions.DocumentOptions.Company = "ST Genetics";
            gridXlExportOptions.DocumentOptions.Author = "Jairo Cesar Fonseca Guerrero";

            await SellsGrid.ExportToXlsxAsync("Animals Sold " + System.DateTime.Now.ToString("ddMMMyyyy-hhmmsstt"), gridXlExportOptions);
        }

        void OnSelectedDataItemsChanged(IReadOnlyList<object> newSelection)
        {
            if (newSelection is IGridSelectionChanges changes)
            {
                if (newSelection.Count == this.AnimalList.Count)
                {
                    foreach (var item in newSelection)
                    {
                        (item as AnimalViewModel).DiscountApplied = false;
                    }

                    (newSelection[4] as AnimalViewModel).DiscountApplied = true;
                    Discount5PercentAmount = (newSelection[4] as AnimalViewModel).Price * (decimal).05;
                }
                else
                {
                    if (newSelection.Count > 5)
                    {
                        if (newSelection.FirstOrDefault(animal => (animal as AnimalViewModel).DiscountApplied == true) == null)
                        {
                            if (SelectedDataItems?.Count > 0)
                            {
                                var lastSelectedDataItem = (newSelection.Except(SelectedDataItems));

                                if (lastSelectedDataItem?.Count() > 0)
                                {
                                    (lastSelectedDataItem.First() as AnimalViewModel).DiscountApplied = true;
                                    Discount5PercentAmount = (lastSelectedDataItem.First() as AnimalViewModel).Price * (decimal).05;
                                }
                                else
                                {
                                    if (changes.DeselectedDataItems.FirstOrDefault(animal => (animal as AnimalViewModel).DiscountApplied == true) != null)
                                    {
                                        foreach (var item in newSelection)
                                        {
                                            (item as AnimalViewModel).DiscountApplied = false;
                                        }

                                        (newSelection[4] as AnimalViewModel).DiscountApplied = true;
                                        Discount5PercentAmount = (newSelection[4] as AnimalViewModel).Price * (decimal).05;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var item in newSelection)
                                {
                                    (item as AnimalViewModel).DiscountApplied = false;
                                }

                                (newSelection[4] as AnimalViewModel).DiscountApplied = true;
                                Discount5PercentAmount = (newSelection[4] as AnimalViewModel).Price * (decimal).05;
                            }
                        }
                    }
                    else
                    {
                        Discount5PercentAmount = 0;

                        foreach (var item in newSelection)
                        {
                            (item as AnimalViewModel).DiscountApplied = false;
                        }
                    }
                }
            }

            SelectedDataItems = newSelection;
        }
    }
}
