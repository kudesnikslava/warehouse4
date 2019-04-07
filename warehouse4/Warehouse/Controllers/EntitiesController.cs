using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CommonLibrary.Models;
using CommonLibrary.Models.Errors;
using CommonLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Models;
using Warehouse.Models.Requests;
using Warehouse.Models.Responses;

namespace Warehouse.Controllers
{
	[Route("/api/v1/entities")]
	public class EntitiesController : Controller
	{
		private DataManager _dataManager;
		public EntitiesController(DataManager dataManager)
		{
			_dataManager = dataManager;
		}

		/// <summary>
		/// Get all entities
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(List<EntityResponse>), 200)]
		public IActionResult GetAllEntities()
		{
			return Ok(_dataManager.GetAllEntities().Select(Mapper.Map<EntityResponse>).ToList());
		}

		/// <summary>
		/// Get entity by id
		/// </summary>
		/// <param name="entityId"></param>
		/// <returns></returns>
		[HttpGet("{entityId}")]
		[ProducesResponseType(typeof(EntityResponse), 200)]
		[ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
		public IActionResult GetEntityById([FromRoute]string entityId)
		{
			Entity entity = _dataManager.GetEntityById(entityId);
			if (entity == null)
				return NotFound(new NotFoundErrorResponse($"entity id: {entityId}"));

			return Ok(Mapper.Map<EntityResponse>(entity));
		}

		/// <summary>
		/// Create entity
		/// </summary>
		/// <param name="entityCreateRequest"></param>
		[HttpPost]
		[ProducesResponseType(typeof(EntityResponse), 200)]
		[ProducesResponseType(typeof(BadRequestResponse), 400)]
		public IActionResult CreateCustomer([FromBody] EntityCreateRequest entityCreateRequest)
		{
			if (entityCreateRequest == null)
			{
				return BadRequest(new BadRequestResponse("invalid model"));
			}
			if (String.IsNullOrEmpty(entityCreateRequest.Name))
			{
				return BadRequest(new BadRequestResponse("Empty Name"));
			}
			//if (String.IsNullOrEmpty(entityCreateRequest.AvailableQuantity))
			//{
			//	return BadRequest(new BadRequestResponse("Empty AvailableQuantity"));
			//}


			Entity newEntity = Mapper.Map<Entity>(entityCreateRequest);
			newEntity.Id = Guid.NewGuid().ToString("N");
			_dataManager.CreateEntity(newEntity);

			return Ok(Mapper.Map<EntityResponse>(newEntity));
		}

		/// <summary>
		/// Updates or creates entity
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="entityUpdateRequest"></param>
		/// <returns></returns>
		[HttpPut("{entityId}")]
		[ProducesResponseType(typeof(EntityResponse), 200)]
		[ProducesResponseType(typeof(BadRequestResponse), 400)]
		public IActionResult UpdateOrCreateEntity(string entityId, [FromBody] EntityUpdateRequest entityUpdateRequest)
		{
			if (entityUpdateRequest == null)
			{
				return BadRequest(new BadRequestResponse("invalid model"));
			}

			if (entityUpdateRequest.Id != entityId)
			{
				return BadRequest(new BadRequestResponse("different id in body and path"));
			}
			if (String.IsNullOrEmpty(entityUpdateRequest.Id))
			{
				return BadRequest(new BadRequestResponse("empty id"));
			}
			if (String.IsNullOrEmpty(entityUpdateRequest.Name))
			{
				return BadRequest(new BadRequestResponse("empty FirstName"));
			}
			//if (String.IsNullOrEmpty(customerUpdateRequest.LastName))
			//{
			//	return BadRequest(new BadRequestResponse("empty LastName"));
			//}

			Entity entity = Mapper.Map<Entity>(entityUpdateRequest);
			_dataManager.UpdateOrCreateEntity(entity);

			return Ok(Mapper.Map<EntityResponse>(entity));
		}

		/// <summary>
		/// Removes entity
		/// </summary>
		/// <param name="entityId"></param>
		/// <returns></returns>
		[HttpDelete("{entityId}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public IActionResult DeleteEntityById([FromRoute]string entityId)
		{
			bool res = _dataManager.RemoveEntity(entityId);
			if (!res)
				return NotFound(new NotFoundErrorResponse($"entity with id {entityId}"));

			//var count = DataProvider.Instance.Entities.RemoveAll(f => f.Id == entityId);
			//if (count < 1)
			//	return NotFound(new NotFoundErrorResponse($"entity with id {entityId}"));

			return Ok();
		}

		/// <summary>
		/// Checks if entity exists
		/// </summary>
		/// <param name="entityId"></param>
		/// <returns></returns>
		[HttpHead("{entityId}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public IActionResult EntityExists([FromRoute] string entityId)
		{
			bool isExists = _dataManager.EntityrExists(entityId);
			//bool isExists = DataProvider.Instance.Entities.Any(f => f.Id == entityId);
			if (!isExists)
				return NotFound(new NotFoundErrorResponse($"entity with id {entityId}"));

			return Ok();
		}
	}
}