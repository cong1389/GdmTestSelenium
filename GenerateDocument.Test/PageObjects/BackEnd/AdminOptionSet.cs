using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Common.WebElements;
using OpenQA.Selenium;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminOptionSet : PageBaseObject
    {
        private readonly ElementLocator
            _footerLinkLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody__XF_DND_OptionSet1_grid_ctl31_hlEditField"),
            _goBackLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_GoBack");

        public AdminOptionSet(DriverContext driverContext) : base(driverContext)
        {
        }

        public void ClickToFooterLink()
        {
            Driver.GetElement<Button>(_footerLinkLocator).ClickTo();
        }

        public void ClickToGoback()
        {
            Driver.GetElement<Button>(_goBackLocator).ClickTo();
        }
    }
}
