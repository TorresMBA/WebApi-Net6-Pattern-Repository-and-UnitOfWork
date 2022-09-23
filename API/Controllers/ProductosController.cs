using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductosController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ProductosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoListDto>>> Get()
        {
            var productos = await _unitOfWork.Producto.GetAllAsync();

            return _mapper.Map<List<ProductoListDto>>(productos);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Get(int id)
        {
            var producto = await _unitOfWork.Producto.GetByIdAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProductoDto>(producto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Post(Producto producto)
        {
            _unitOfWork.Producto.Add(producto);
            await _unitOfWork.SaveAsync();

            if (producto == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(Post), new { id = producto.Id }, producto);
        }

        //PUT: api/productos/4
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Producto>> Put(int id, [FromBody]Producto producto)
        {
            if (producto == null)
                return NotFound();
            
            _unitOfWork.Producto.Update(producto);
            await _unitOfWork.SaveAsync();

            return producto;
        }

        //DELETE: api/productos
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var producto = await _unitOfWork.Producto.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _unitOfWork.Producto.Remove(producto);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

    }
}
