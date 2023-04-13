// Seleccionar elementos del DOM
const listBtn = document.querySelector('.list-btn');
const kanbanBtn = document.querySelector('.kanban-btn');
const listContent = document.querySelector('.list-view');
const kanbanContent = document.querySelector('.kanban-view');

// Funciones para cambiar entre vista de lista y vista de kanban
function showList() {
    listBtn.classList.add('active');
    kanbanBtn.classList.remove('active');
    listContent.style.display = 'block';
    kanbanContent.style.display = 'none';
}

function showKanban() {
    kanbanBtn.classList.add('active');
    listBtn.classList.remove('active');
    kanbanContent.style.display = 'block';
    listContent.style.display = 'none';
}

// Añadir event listeners a los botones de cambio de vista
listBtn.addEventListener('click', showList);
kanbanBtn.addEventListener('click', showKanban);

const createBtn = document.querySelector('.btn-create');
const modal = document.querySelector('.modal');
const closeBtn = document.querySelector('.close-btn');

createBtn.addEventListener('click', () => {
    modal.style.display = 'block';
});

closeBtn.addEventListener('click', () => {
    modal.style.display = 'none';
});