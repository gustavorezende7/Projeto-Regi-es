const API_BASE = "http://localhost:5000"; 
const form = document.getElementById('form-regiao');
const ufEl = document.getElementById('uf');
const nomeEl = document.getElementById('nome');
const tabelaBody = document.querySelector('#tabela-regioes tbody');
const incluirInativasEl = document.getElementById('incluirInativas');
const toast = document.getElementById('toast');

function showToast(msg, ms = 2500){
  toast.textContent = msg;
  toast.style.display = 'block';
  setTimeout(()=> toast.style.display='none', ms);
}

async function carregarRegioes(){
  const incluir = incluirInativasEl.checked ? '?incluirInativas=true' : '';
  const res = await fetch(API_BASE + '/regioes' + incluir);
  if(!res.ok){ showToast('Erro ao carregar regiões'); return; }
  const data = await res.json();
  tabelaBody.innerHTML = '';
  data.forEach(r => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
      <td>${r.uf}</td>
      <td>${r.nome}</td>
      <td>${r.ativo ? '<span class="ativo">Ativo</span>' : '<span class="inativo">Inativo</span>'}</td>
      <td>
        <a href="#" data-id="${r.id}" class="acao editar">Editar</a>
        ${r.ativo ? `<a href="#" data-id="${r.id}" class="acao inativar">Inativar</a>` : `<a href="#" data-id="${r.id}" class="acao ativar">Ativar</a>`}
      </td>`;
    tabelaBody.appendChild(tr);
  });
  attachActions();
}

function attachActions(){
  document.querySelectorAll('.editar').forEach(a=>{
    a.addEventListener('click', async (e)=>{
      e.preventDefault();
      const id = a.dataset.id;
      const res = await fetch(API_BASE + '/regioes/' + id);
      if(!res.ok){ showToast('Não foi possível buscar região'); return; }
      const r = await res.json();
      ufEl.value = r.uf;
      nomeEl.value = r.nome;
      form.dataset.editId = r.id;
      document.getElementById('btn-submit').value = 'Salvar';
      window.scrollTo({top:0,behavior:'smooth'});
    });
  });

  document.querySelectorAll('.inativar').forEach(a=>{
    a.addEventListener('click', async (e)=>{
      e.preventDefault();
      const id = a.dataset.id;
      const ok = confirm('Deseja inativar esta região?');
      if(!ok) return;
      const res = await fetch(API_BASE + '/regioes/' + id + '?ativo=false', { method: 'PATCH' });
      if(res.ok){ showToast('Região inativada'); carregarRegioes(); } else showToast('Erro ao inativar');
    });
  });

  document.querySelectorAll('.ativar').forEach(a=>{
    a.addEventListener('click', async (e)=>{
      e.preventDefault();
      const id = a.dataset.id;
      const res = await fetch(API_BASE + '/regioes/' + id + '?ativo=true', { method: 'PATCH' });
      if(res.ok){ showToast('Região ativada'); carregarRegioes(); } else showToast('Erro ao ativar');
    });
  });
}

form.addEventListener('submit', async (e)=>{
  e.preventDefault();
  const uf = ufEl.value.trim();
  const nome = nomeEl.value.trim();
  if(!uf || !nome){ showToast('Preencha todos os campos'); return; }

  const editId = form.dataset.editId;
  if(editId){
    // atualizar via PUT
    const res = await fetch(API_BASE + '/regioes/' + editId, {
      method: 'PUT',
      headers: { 'Content-Type':'application/json' },
      body: JSON.stringify({ uf, nome, ativo: true })
    });
    if(res.status === 204){ showToast('Atualizado com sucesso'); } 
    else if(res.status === 409){ showToast('Conflito: já existe região com esse nome/UF'); }
    else { showToast('Erro ao atualizar'); }
    delete form.dataset.editId;
    document.getElementById('btn-submit').value = 'Inserir';
  } else {
    // criar via POST
    const res = await fetch(API_BASE + '/regioes', {
      method: 'POST',
      headers: { 'Content-Type':'application/json' },
      body: JSON.stringify({ uf, nome })
    });
    if(res.status === 201){ showToast('Criado com sucesso'); } 
    else if(res.status === 409){ showToast('Conflito: já existe região com esse nome/UF'); }
    else { 
      const text = await res.text();
      showToast('Erro ao criar: ' + text);
    }
  }

  ufEl.value=''; nomeEl.value=''; carregarRegioes();
});

incluirInativasEl.addEventListener('change', carregarRegioes);

// initial load
carregarRegioes();
