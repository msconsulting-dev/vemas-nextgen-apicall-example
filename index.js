// Import stylesheets
import "./style.css";

const API_TOKEN = "330BD50B-88EE-47FF-A12B-7B579F50772B";
//const ENDPOINT1 = "http://localhost:54401/api/Address";
//const ENDPOINT2 = "http://localhost:54401/api/InsideDemo";
const ENDPOINT1 = "https://kundenserver.msconsulting.de/Inside.Api/api/Address";
const ENDPOINT2 =
  "https://kundenserver.msconsulting.de/Inside.Api/api/InsideDemo";

document.getElementById("api_token").value = API_TOKEN;
document.getElementById("endpoint1").value = ENDPOINT1;
document.getElementById("endpoint2").value = ENDPOINT2;

async function addAddress() {
  let payload = {
    data: {
      "Firma_1": null,
      "Strasse": null,
      "PLZ": null,
      "Ort": null
    }
  };

  payload.data.Firma_1 = document.getElementById("firma_1").value;
  payload.data.Strasse = document.getElementById("strasse").value;
  payload.data.PLZ = document.getElementById("plz").value;
  payload.data.Ort = document.getElementById("ort").value;

  let api_token = document.getElementById("api_token").value;
  let endpoint = document.getElementById("endpoint1").value;

  apiCall(payload, endpoint, api_token).then();
}

async function callPlugin() {
  let payload = {
    "Firma1": null,
    "Strasse": null,
    "PLZ": null,
    "Ort": null,
    "Vorname": null,
    "Nachname": null,
    "Email": null
  };

  payload.Firma1 = document.getElementById("firma_1-plugin").value;
  payload.Strasse = document.getElementById("strasse-plugin").value;
  payload.PLZ = document.getElementById("plz-plugin").value;
  payload.Ort = document.getElementById("ort-plugin").value;
  payload.Vorname = document.getElementById("vorname-plugin").value;
  payload.Nachname = document.getElementById("nachname-plugin").value;
  payload.Email = document.getElementById("email-plugin").value;

  let api_token = document.getElementById("api_token").value;
  let endpoint = document.getElementById("endpoint2").value;

  apiCall(payload, endpoint, api_token).then();
}

async function apiCall(payload, endpoint, api_token) {
  let alertS = document.getElementById("alertSuccess");
  let alertD = document.getElementById("alertDanger");

  alertS.classList.add("d-none");
  alertD.classList.add("d-none");

  let apiCall = fetch(endpoint, {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
      Authorization: `APITOKEN ${api_token}`
    },
    body: JSON.stringify(payload)
  });

  apiCall
    .then(res => {
      // if we are ok, return the body.json as result
      if (res.status == 200) {
        return res.json();
      }

      // if we are not okay but we have a response body (when throwing errors at the backend with a response)
      // we return null (to prevent a success message) and show a message
      // if there is a "real" error, we throw it
      return res
          .json()
          .then(json => {
            showMessage("Error", alertD, json);
            return null
          })

          // if there is a real "error" we throw it
          .catch(exception => {
            throw new Error(res.statusText);
          });
    })
    .then(responseBody => {
      if (responseBody) {
        showMessage("Success", alertS, responseBody);
      }
    })
    .catch(error => {
      showMessage("Error", alertD, `Server responded with ${error}`);
    });
}

function showMessage(title, element, message) {
  element.innerHTML = "";

  const titleDiv = document.createElement("div");
  titleDiv.innerHTML = title;
  titleDiv.classList.add("font-weight-bold");

  element.classList.toggle("d-none");
  element.appendChild(titleDiv);

  let messageDiv = document.createElement("div");

  if (typeof message == "object") {
    const preElement = document.createElement("pre");
    const codeElement = document.createElement("code");

    codeElement.innerHTML = JSON.stringify(message, null, 2);
    preElement.appendChild(codeElement);

    messageDiv.appendChild(preElement);
  } else {
    messageDiv.innerHTML = message;
  }

  element.appendChild(messageDiv);
}

// this is only needed for stackblitz enviroment
// https://stackoverflow.com/questions/52234484/stackblitz-onclick-doesnt-find-my-function
window.apiCall = apiCall;
window.addAddress = addAddress;
window.callPlugin = callPlugin;
