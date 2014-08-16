namespace SodokuSolver.Logic

open System.Web.Http
open Newtonsoft.Json.Serialization
open global.Owin
open Microsoft.Owin

type Config = {
    id : RouteParameter
}

type Startup() =
    member __.Configuration(app:IAppBuilder) =
        let config =
            let config = new HttpConfiguration()
            config.Formatters.Remove config.Formatters.XmlFormatter |> ignore
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver <- DefaultContractResolver()
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", { id = RouteParameter.Optional }) |> ignore
            config
    
        app.UseWebApi config |> ignore