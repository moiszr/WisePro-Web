document.getElementById('input-titulo').addEventListener('click', function () {
    document.getElementById('input-descripcion').style.display = 'block';
    document.getElementById('button-add').style.display = 'block';
});

document.addEventListener('click', function (event) {
    var formulario = document.getElementById('formulario-notas');
    var fueraDelFormulario = !formulario.contains(event.target);
    if (fueraDelFormulario) {
        document.getElementById('input-descripcion').style.display = 'none';
        document.getElementById('button-add').style.display = 'none';
    }
});