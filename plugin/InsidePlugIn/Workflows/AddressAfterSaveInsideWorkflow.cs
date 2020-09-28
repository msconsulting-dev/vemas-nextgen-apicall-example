using System;
using System.Collections.Generic;
using System.Text;
using vng.core.Interfaces;

namespace vng.plugin.insideplugin.Workflows
{
    public class AddressAfterSaveInsideWorkflow : IDataWorkflowAfterSave
    {
        private readonly IDataBaseFactory dataBaseFactory;

        public AddressAfterSaveInsideWorkflow(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }


        public IList<string> DependentFields => new[] { "Adressherkunft", "Nummer", "Zusatzstichwort" };



        public eDataWorkflowResult Execute(ICRUDWorkflowDataBase data, bool wasNew)
        {
            string adressherkunft = data.GetDataField<string>(this, "Adressherkunft");
            int accountId = data.GetID();
            string zusatzstichwort = data.GetDataField<string>(this, "Zusatzstichwort");

            if (wasNew && adressherkunft == "Inside")
            {
                IDataBase acquisition = this.dataBaseFactory.GetDataBase("ACQUISITION");

                acquisition.Defaults = new Dictionary<string, object>();
                acquisition.Defaults.Add("Nummer", accountId);

                if (!acquisition.Load(null, true, data.CurrentAgent))
                    return eDataWorkflowResult.OKResumeNext;

                acquisition.GetDictData()["Person"] = "SZS";
                acquisition.GetDictData()["Betreff"] = "Homepage - neue Adresse";
                acquisition.GetDictData()["AkquisitionHTML"] = $"<p>{zusatzstichwort}</p>";

                if (!acquisition.Save())
                    return eDataWorkflowResult.OKResumeNext;
            }

            return eDataWorkflowResult.OKResumeNext;
        }
    }
}
