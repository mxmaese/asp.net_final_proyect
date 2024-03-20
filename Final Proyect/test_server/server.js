"use strict";

const banner = require("node-banner");
const express = require("express");
const fsex = require("fs-extra");
const delay = require("delay");
const fs = require("fs");
const server = express();

let NEXTID = 1000;

banner("Server API", "Para simular accesos a las API de libreriaPTR");

const cors = require("cors");

const port = 3030;

server.use(cors());
server.use(express.json());
server.use(express.urlencoded({ extended: true }));

var timeout = null;

var datosFull = {};
var datosSearch = {};
var datosTecnicos = {};
var tiposEvento = {};

/**
 * Lectura de archivos para enviar como response
 * 
 */
fsex.readJson("datos.json", "latin1", (err, obj) => {
  datosFull = obj;
});

fsex.readJson("search_result.json", "latin1", (err, obj) => {
  datosSearch = obj;
});

/**
 * Endpoints del servidor http simulado
 */
server.listen(port, () => {
  console.log("Esperando request...");
});

server.get("/", (req, res) => {
  console.log("recibido");
  res.status(500).send("OK");
});

server.get("/api/libros", (request, response) => {
  console.log(request.url);
  console.dir(request.query);

  setTimeout(() =>
  {
    response.set({
      "Content-Type": "application/json; charset=utf-8",
      "X-Powered-By": "NodeJS",
    });
    response.status(200).send(datosFull);
    //  si queremos provocar un error, reemplazar el response previo por el codigo de abajo
    //  ademas de retornar un status code de error podemos incluir datos que estarian en el
    //  body del mensaje http
    //
    //  response.status(400).send({
    //    causa: "Paso algo",
    //    subCodigo: 123456
    //  });
  }, 3000);
});


