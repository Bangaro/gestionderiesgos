// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Escuchar el evento 'popstate' para detectar la navegación hacia atrás
// Función para establecer el estado de navegación al cargar la página principal
document.addEventListener("DOMContentLoaded", function() {
    // Obtener la URL actual
    var currentUrl = window.location.pathname;

    // Definir la URL que activará el ícono "previous"
    var targetUrl = "/Riesgos/SeleccionarRiesgo";

    // Obtener referencias a los elementos de los íconos
    var previousIcon = document.getElementById("previousIcon");
    var otherIcon = document.getElementById("otherIcon");

    // Mostrar el ícono correspondiente según la URL actual
    if (currentUrl === targetUrl) {
        // Mostrar el ícono "previous"
        previousIcon.style.display = "inline-block";
        otherIcon.style.display = "none";
    } else {
        // Mostrar el otro ícono
        previousIcon.style.display = "none";
        otherIcon.style.display = "inline-block";
    }
});
