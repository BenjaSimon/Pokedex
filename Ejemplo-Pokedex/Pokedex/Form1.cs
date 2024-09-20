using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using Negocio;

namespace Pokedex
{
    public partial class Pokedex : Form
    {
        private List<Pokemon> listaPokemon;
        public Pokedex()
        {
            InitializeComponent();
        }

        private void Pokedex_Load(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            listaPokemon = negocio.listar();
            dgvPokemons.DataSource = listaPokemon;
            dgvPokemons.Columns["UrlImagen"].Visible = false;
            cargarimagen(listaPokemon[0].UrlImagen);
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
            cargarimagen(seleccionado.UrlImagen);
        }
        private void cargarimagen(string imagen)
        {
            try
            {
                pbPokemon.Load(imagen);

            }
            catch (Exception)
            {

                pbPokemon.Load("https://cdn3.iconfinder.com/data/icons/web-development-and-programming-2/64/development_Not_Found-1024.png");
            }
        }
    }
}
