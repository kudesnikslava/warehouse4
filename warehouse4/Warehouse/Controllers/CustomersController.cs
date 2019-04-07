using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CommonLibrary.Models;
using CommonLibrary.Models.Errors;
using CommonLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Models;
using Warehouse.Models.Requests;
using Warehouse.Models.Responses;

namespace Warehouse.Controllers
{
	/// <summary>
	/// Handles customers
	/// </summary>
	[Route("/api/v1/customers")]
	public class CustomersController : Controller
	{
		private DataManager _dataManager;

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="dataManager"></param>
		public CustomersController(DataManager dataManager) //
		{
			_dataManager = dataManager;
		}

		/// <summary>
		/// Get all customers
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(List<CustomerResponse>), 200)]
		public IActionResult GetAllCustomers()
		{
			List<Customer> customers = _dataManager.GetAllCustomers();

			return Ok(customers.Select(Mapper.Map<CustomerResponse>));
		}

		/// <summary>
		/// Get customer by id
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		[HttpGet("{customerId}")]
		[ProducesResponseType(typeof(CustomerResponse), 200)]
		[ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
		public IActionResult GetCustomerById([FromRoute]string customerId)
		{
			Customer customer = _dataManager.GetCustomerById(customerId).Result;
			if (customer == null)
				return NotFound(new NotFoundErrorResponse($"customer id: {customerId}"));

			return Ok(Mapper.Map<CustomerResponse>(customer));
		}

		/// <summary>
		/// Creates customer
		/// </summary>
		/// <param name="customerCreateRequest"></param>
		[HttpPost]
		[ProducesResponseType(typeof(CustomerResponse), 200)]
		[ProducesResponseType(typeof(BadRequestResponse), 400)]
		public IActionResult CreateCustomer([FromBody] CustomerCreateRequest customerCreateRequest)
		{
			if (customerCreateRequest == null)
			{
				return BadRequest(new BadRequestResponse("invalid model"));
			}
			if (String.IsNullOrEmpty(customerCreateRequest.FirstName))
			{
				return BadRequest(new BadRequestResponse("Empty First Name"));
			}
			if (String.IsNullOrEmpty(customerCreateRequest.LastName))
			{
				return BadRequest(new BadRequestResponse("Empty Last Name"));
			}


			Customer newCustomer = Mapper.Map<Customer>(customerCreateRequest);
			//newCustomer.Id = Guid.NewGuid().ToString("N");
			//DataProvider.Instance.AddCustomer(newCustomer);

			_dataManager.CreateCustomer(newCustomer);

			return Ok(Mapper.Map<CustomerResponse>(newCustomer));
		}

		/// <summary>
		/// Updates or creates customer
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="customerUpdateRequest"></param>
		/// <returns></returns>
		[HttpPut("{customerId}")]
		[ProducesResponseType(typeof(CustomerResponse), 200)]
		[ProducesResponseType(typeof(BadRequestResponse), 400)]
		public IActionResult UpdateOrCreateCustomer(string customerId, [FromBody] CustomerUpdateRequest customerUpdateRequest)
		{
			if (customerUpdateRequest == null)
			{
				return BadRequest(new BadRequestResponse("invalid model"));
			}

			if (customerUpdateRequest.Id != customerId)
			{
				return BadRequest(new BadRequestResponse("different id in body and path"));
			}
			if (String.IsNullOrEmpty(customerUpdateRequest.Id))
			{
				return BadRequest(new BadRequestResponse("empty id"));
			}
			if (String.IsNullOrEmpty(customerUpdateRequest.FirstName))
			{
				return BadRequest(new BadRequestResponse("empty FirstName"));
			}
			if (String.IsNullOrEmpty(customerUpdateRequest.LastName))
			{
				return BadRequest(new BadRequestResponse("empty LastName"));
			}

			Customer customer = Mapper.Map<Customer>(customerUpdateRequest);
			//DataProvider.Instance.AddOrUpdateCustomer(customer);

			_dataManager.UpdateOrCreateCustomer(customer);

			return Ok(Mapper.Map<CustomerResponse>(customer));
		}

		/// <summary>
		/// Removes customer
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		[HttpDelete("{customerId}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public IActionResult DeleteCustomerById([FromRoute]string customerId)
		{
			bool res =_dataManager.RemoveCustomer(customerId);
			if (!res)
				return NotFound(new NotFoundErrorResponse($"customer with id {customerId}"));

			//var count = DataProvider.Instance.Customers.RemoveAll(f => f.Id == customerId);
			//if (count < 1)
			//	return NotFound(new NotFoundErrorResponse($"customer with id {customerId}"));

			//TODO Почему ответ CustomerResponse, а он не возвращается? как правильно оформить тип ответа при return Ok();
			return Ok();
		}

		/// <summary>
		/// Checks if customer exists
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		[HttpHead("{customerId}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public IActionResult CustomerExists([FromRoute] string customerId)
		{
			bool isExists = _dataManager.CustomerExists(customerId);
			//bool isExists = DataProvider.Instance.Customers.Any(f => f.Id == customerId);
			if (!isExists)
				return NotFound(new NotFoundErrorResponse($"customer with id {customerId}"));

			//TODO Почему ответ CustomerResponse, а он не возвращается? как правильно оформить тип ответа при return Ok();
			return Ok();
		}
	}
}