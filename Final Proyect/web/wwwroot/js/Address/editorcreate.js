function mostrarError(inputId, mensaje) {
    const input = document.getElementById(inputId);
    const mensajeError = document.getElementById(inputId + '_O');

    if (input && mensajeError) {
        input.classList.add('input-error');

        mensajeError.textContent = mensaje;
    }
}

function limpiarError(inputId) {
    const input = document.getElementById(inputId);
    const mensajeError = document.getElementById(inputId + '_O');

    if (input && mensajeError) {
        input.classList.remove('input-error');

        mensajeError.textContent = '';
    }
}

document.addEventListener('DOMContentLoaded', function () {


    function validateMap(lat, lng) {
        console.log("Validando mapa con latitud: ", lat, " y longitud: ", lng);
    }

    function validateFormFields() {
        console.log("Validando campos de formulario");
        var type = document.getElementById('type').value;
        if (type == "0") {
            var Country = document.getElementById('Country').value;
            var State = document.getElementById('State').value;
            var City = document.getElementById('City').value;
            var Street = document.getElementById('Street').value;
            var Height = document.getElementById('Height').value;
            var ZipCode = document.getElementById('Zip_code').value;

            var Country_O = document.getElementById('Country_O');
            var State_O = document.getElementById('State_O');
            var City_O = document.getElementById('City_O');
            var Street_O = document.getElementById('Street_O');
            var Height_O = document.getElementById('Height_O');
            var ZipCode_O = document.getElementById('Zip_code_O');
            
            var isValid = true;
            if (!Country) {
                console.log("no se completo el campo pais");
                mostrarError('Country', "El campo Pais es obligatorio");
                isValid = false;
            }
            else {
                limpiarError("Country");
            }
            if (!State) {
                console.log("no se completo el campo state");
                mostrarError('State', "El campo Provincia es obligatorio");
                isValid = false;
            } else {
                limpiarError("State");
            }
            if (!City) {
                console.log("no se completo el campo city");
                mostrarError('City', "El campo City es obligatorio");
                isValid = false;
            } else {
                limpiarError("City");
            }
            if (!Street) {
                console.log("no se completo el campo street");
                mostrarError('Street', "El campo Street es obligatorio");
                isValid = false;
            } else {
                limpiarError("Street");
            }
            if (!Height) {
                console.log("no se completo el campo height");
                mostrarError('Height', "El campo Altura es obligatorio");
                isValid = false;
            } else {
                limpiarError("Height");
            }
            if (!ZipCode) {
                console.log("no se completo el campo zip code");
                mostrarError('Zip_code', "El campo Codigo postal es obligatorio");
                isValid = false;
            } else {
                if (!validateSpecificFormat(ZipCode)) {
                    console.log("no se introdujo correctamente el campo zip code");
                    mostrarError('Zip_code', "El campo Codigo postal debe tener un formato de  LNNNN");
                    isValid = false;
                } else {
                    limpiarError("Zip_code");
                }
            }
            if (!isValid) {
                return false;
            }
            return true;
        } else if (type == "1") {
            var longitude = document.getElementById('longitude').value;
            var latitude = document.getElementById('latitude').value;
            return true;
        }
    }

    function validateSpecificFormat(value) {
        return /^[A-Za-z][0-9]{4}$/.test(value);
    }

    document.getElementById('frmUsuario').onsubmit = function (e) {
        e.preventDefault(); 

        if (!validateFormFields()) {
            console.log("no se puedo")
            return false; 
        } else {
            $("#Line_1O").val(" ");
            $("#Line_2O").val(" ");

            this.submit();
        }

        console.log("Formulario válido, procediendo a enviar...");
    };
});
