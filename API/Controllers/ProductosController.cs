using API.Dtos;
using API.Helpers;
using API.Helpers.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProductosController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductosController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _logger.LogInformation("Estamos en el constructor de 'ProductosController'");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<ProductoListDto>>> Get([FromQuery] Params productParams)
        {
            var resultado = await _unitOfWork.Producto.GetAllAsync(productParams.PageIndex, productParams.PageSize, productParams.Search);

            var listaProductosDto = _mapper.Map<List<ProductoListDto>>(resultado.registros);

            return new Pager<ProductoListDto>(listaProductosDto, resultado.totalRegistros, productParams.PageIndex, productParams.PageSize, productParams.Search);
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
                return NotFound(new ApiResponse(404, "El producto solicitado no existe."));
            }

            return _mapper.Map<ProductoDto>(producto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoAddUpdateDto>> Post(ProductoAddUpdateDto productoDto)
        {
            var producto = _mapper.Map<Producto>(productoDto);

            _unitOfWork.Producto.Add(producto);
            await _unitOfWork.SaveAsync();

            if (producto == null)
            {
                return BadRequest(new ApiResponse(500, ""));
            }
            productoDto.Id = producto.Id;
            return CreatedAtAction(nameof(Post), new { id = productoDto.Id }, productoDto);
        }

        //PUT: api/productos/4
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoAddUpdateDto>> Put(int id, [FromBody]ProductoAddUpdateDto productoDto)
        {
            if (productoDto == null)
                return NotFound(new ApiResponse(400));

            var productobd = await _unitOfWork.Producto.GetByIdAsync(id);

            if (productobd == null)
            {
                return NotFound(new ApiResponse(404, "El producto solicitado no existe."));
            }

            var producto = _mapper.Map<Producto>(productoDto);

            _unitOfWork.Producto.Update(producto);
            await _unitOfWork.SaveAsync();

            return productoDto;
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
                return NotFound(new ApiResponse(404, "El producto solicitado no existe."));
            }

            _unitOfWork.Producto.Remove(producto);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

    }
}
