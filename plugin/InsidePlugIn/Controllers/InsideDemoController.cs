using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vng.core;
using vng.core.Helpers;
using vng.core.Interfaces;
using vng.core.Models;
using vng.plugin.insideplugin.Models;

namespace vng.plugin.insideplugin.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = SwaggerGroup.PLUGIN)]
    [Authorize]
    public class InsideDemoController : ControllerBase
    {
        private readonly IDataBaseFactory dataBaseFactory;
        private readonly ITokenService tokenService;

        public InsideDemoController(IDataBaseFactory dataBaseFactory, ITokenService tokenService)
        {
            this.dataBaseFactory = dataBaseFactory;
            this.tokenService = tokenService;
        }



        [HttpPost]
        public IActionResult AdresseKontakt([FromBody] AddressContactData data)
        {
            IDataBase address = this.dataBaseFactory.GetDataBase("ADDRESS");
            if (!address.Load(null, true, this.getCurrentAgent()))
                return this.getError(address, "Fehler, keine neue Adresse möglich");

            address.GetDictData()["Zusatzstichwort"] = data.Firma1;
            address.GetDictData()["Firma_1"] = data.Firma1;
            address.GetDictData()["Email"] = data.Email;
            address.GetDictData()["Strasse"] = data.Strasse;
            address.GetDictData()["PLZ"] = data.PLZ;
            address.GetDictData()["Ort"] = data.Ort;
            address.GetDictData()["Adressherkunft"] = "Inside";

            if (!address.Save())
                return this.getError(address, "Fehler, die Adresse konnte nicht gespeichert werden");

            int accountId = address.GetID();
            address.GetDictData().TryFindKeyAndGetValue("Kundennummer", out int kundennummer);



            IDataBase contact = this.dataBaseFactory.GetDataBase("CONTACT");
            contact.Defaults = new Dictionary<string, object>();
            contact.Defaults.Add("Nummer", accountId);

            if (!contact.Load(null, true, this.getCurrentAgent()))
                return this.getError(contact, "Fehler, kein neuer Kontakt möglich");

            contact.GetDictData()["Vorname_Kontaktperson"] = data.Vorname;
            contact.GetDictData()["Name_Kontaktperson"] = data.Nachname;
            contact.GetDictData()["EMail"] = data.Email;

            if (!contact.Save())
                return this.getError(contact, "Fehler, der Kontakt konnte nicht gespeichert werden");


            return new OkObjectResult($"Ihre Kundennummer: {kundennummer}");
        }




        private string getCurrentAgent()
        {
            return this.tokenService.ClaimValue(this.HttpContext.User.Claims, VemasTokenClaims.CLAIM_CODE);
        }

        private IActionResult getError(IDataBase dataBase, string msg)
        {
            if (dataBase == null) return new BadRequestObjectResult(msg);
            return new BadRequestObjectResult($"<p>{msg}</p><p><i>{((ICRUDWorkflowDataBase)dataBase).ErrorMessage}</i></p>");
        }
    }
}
