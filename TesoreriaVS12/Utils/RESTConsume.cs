using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Utils
{
    public class RESTConsume
    {
        private string UrlWSTespreria;

        public RESTConsume()
        {
            if (String.IsNullOrEmpty(this.UrlWSTespreria)) this.UrlWSTespreria = ConfigurationManager.AppSettings.Get("Url_WS_Tesoreria");
        }

        private IRestResponse ConsumeRest(string Url, RestRequest request)
        {
            RestClient client = new RestClient(Url);
            return client.Execute(request);
        }
        private IRestResponse ConsumeRest(string Url, RestRequest request, List<Headers> headers)
        {
            //Add Headers
            this.GenerateHeaders(headers, ref request);
            RestClient client = new RestClient(Url);
            return client.Execute(request);
        }

        private void GenerateHeaders(List<Headers> headers, ref RestRequest request)
        {
            foreach (Headers element in headers)
            {
                request.AddHeader(element.headerName, element.headerValue);
            }
        }

        private T GenerateJsonObject<T>(string _json)
        {
            return (T)new JavaScriptSerializer().Deserialize(_json, (Type)typeof(T));
        }

        public ResponseCancelacion CancelaRequisicion(int NoRequisicion, string motivo)
        {
            RestRequest request = new RestRequest(String.Format("{0}/{1}.{2}", Enums.Action.borrar, Enums.Methods.requisicion, Enums.Format.json), Method.POST);
            request.AddParameter("no_req", NoRequisicion);
            request.AddParameter("motivo", motivo);

            try
            {
                IRestResponse response = this.ConsumeRest(this.UrlWSTespreria, request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    RestResponseObject<ResponseCancelacion> result = this.GenerateJsonObject<RestResponseObject<ResponseCancelacion>>(response.Content);
                    if (result.response != null && result.response != null)
                        return result.response;
                    return new ResponseCancelacion { error = true, mensaje = response.ErrorMessage };
                }
                else
                {
                    RestResponseObject<ResponseCancelacion> result = this.GenerateJsonObject<RestResponseObject<ResponseCancelacion>>(response.Content);
                    return result.response;
                }
            }
            catch (Exception ex)
            {
                return new ResponseCancelacion() { error = true, mensaje = String.Format("{0}", ex.Message) };
            }
        }

        public ResponseCancelada VerificaCancelacion(int NoRequisicion)
        {
            RestRequest request = new RestRequest(String.Format("{0}/{1}", Enums.Action.obtener, Enums.Methods.requisicion_cancelada/*, Enums.Format.json*/), Method.GET);
            request.AddParameter("no_req", NoRequisicion);            

            try
            {
                IRestResponse response = this.ConsumeRest(this.UrlWSTespreria, request, new List<Headers>() { new Headers { headerName = "Accept", headerValue = "application/json" } });
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    RestResponseObject<ResponseCancelada> result = this.GenerateJsonObject<RestResponseObject<ResponseCancelada>>(response.Content);
                    if (result.response != null && result.response != null)
                    {
                        result.response.error = false;
                        return result.response;
                    }
                    return new ResponseCancelada { error = true, cancelado = false, mensaje = response.ErrorMessage };
                }
                else
                {
                    RestResponseObject<ResponseCancelada> result = this.GenerateJsonObject<RestResponseObject<ResponseCancelada>>(response.Content);
                    result.response.error = false;
                    return result.response;
                }
            }
            catch (Exception ex)
            {
                return new ResponseCancelada() { error = true, cancelado = false, mensaje = String.Format("{0}", ex.Message) };
            }
        }

    }
}