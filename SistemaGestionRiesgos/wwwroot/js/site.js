// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Escuchar el evento 'popstate' para detectar la navegación hacia atrás
// Función para establecer el estado de navegación al cargar la página principal

const toastContent = document.querySelector('.toast');
const toast = new bootstrap.Toast(toastContent);

toast.show();


document.addEventListener("DOMContentLoaded", function() {
    // Obtener la URL actual
    var currentUrl = window.location.pathname;

    // Definir la URL raíz
    var rootUrl = "/";

    // Obtener referencia al elemento del ícono "previous"
    var previousIcon = document.getElementById("previousIcon");

    // Mostrar u ocultar el ícono "previous" según la URL actual
    if (currentUrl === rootUrl) {
        // Si estamos en la URL raíz, ocultar el ícono "previous"
        previousIcon.style.display = "none";
    } else {
        // Si no estamos en la URL raíz, mostrar el ícono "previous"
        previousIcon.style.display = "inline-block";
    }
});

