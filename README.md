# Dinamik Nesne Oluşturma API'si

## Genel Bakış

Bu API, kullanıcıların tek bir dinamik tablo üzerinden çeşitli nesneleri (örneğin Siparişler, Ürünler, Müşteriler) oluşturabileceği, okuyabileceği, güncelleyebileceği ve silebileceği bir sistem sunar. API, farklı nesne türlerini ve bunlara ait alanları dinamik olarak yönetir ve fiziksel veritabanı tablolarını manuel olarak oluşturma gereksinimini ortadan kaldırır. Tüm CRUD işlemleri merkezi bir API geçidi aracılığıyla gerçekleştirilir ve sistem, ilişkili nesnelerin (örneğin, bir Sipariş ve ona ait Ürünler) bir bütün olarak işlenmesini sağlar.

## Özellikler

1. **Dinamik Nesne Oluşturma**:
   - Kullanıcılar, API'ye istek göndererek dinamik olarak yeni nesneler (siparişler, müşteriler ve ürünler gibi) oluşturabilir.
   - Her nesnenin yapısı merkezi bir tabloda saklanır, bu da nesne türleri ve alanlarında esneklik sağlar.

2. **CRUD İşlemleri**:
   - Tek bir API geçidi üzerinden dinamik nesneler oluşturma, okuma, güncelleme ve silme işlemleri gerçekleştirin.
   - API, farklı nesne türlerine uyum sağlar (örneğin, siparişler, ürünler, müşteriler) ve her bir nesne için dinamik alanlar kabul eder.

3. **İşlem Yönetimi**:
   - Bir API isteği birden fazla ilişkili nesneyi (örneğin, bir Sipariş ve Ürünleri) tek bir işlem olarak ele alır.
   - Tüm nesnelerin başarıyla oluşturulmasını veya hata durumunda hiçbirinin oluşturulmamasını sağlamak için işlem yönetimi uygulanır.

4. **Hata Yönetimi**:
   - Geçersiz nesne yapısı, eksik alanlar, veritabanı bağlantı sorunları ve doğrulama hataları gibi durumları yönetir.
   - Kullanıcılara hatanın nedenini açıklayan anlamlı hata mesajları sağlar.

5. **Veri Doğrulama**:
   - Her nesne türü için gerekli alanların mevcut olduğunu doğrular.
   - Doğrulama mantığı, nesne türüne göre uyarlanır (örneğin, Siparişler müşteri kimliği ve en az bir ürün gerektirirken, Ürünler isim ve fiyat gerektirir).

## API Uç Noktaları

- **Create**:  
  `POST https://localhost:5000/api/Base`  
  Yeni kayıtlar oluşturmak için JSON formatında veri gönderin.  
  Örnek JSON isteği:
  ```json
  {
    "DynamicObject": {
      "Customer": {
        "CustomerId": "11111",
        "Name": "test user",
        "Password": "12345"
      }
    },
    "DynamicSubObject": [
      {
        "Order": {
          "CustomerId": "11111",
          "Product": [
            {
              "Name": "Test",
              "Price": 0,
              "Quantity": 1
            },
            {
              "Name": "Test",
              "Price": 0,
              "Quantity": 2
            }
          ]
        }
      }
    ]
  }
  ```

 - **Read**:  
  `Get https://localhost:5000/api/Base`  
  Nesne türü ve filtrelere göre kayıtları getirin (örneğin, belirli bir müşteriye ait tüm siparişleri getirin).

 - **Update**:
  `PUT https://localhost:5000/api/Base`
  Varolan kayıtları güncellemek için JSON formatında veri gönderin.
  Örnek JSON isteği:
  ```json
  {
    "id": 1, 
	"DynamicObject": {
	  "Customer": {
		"CustomerId": "11111",
		"Name": "test user",
		"Password": "12345"
	  }
	},
	"DynamicSubObject": [
	  {
		"Order": {
		  "CustomerId": "11111",
		  "Product": [
			{
			  "Name": "Test",
			  "Price": 0,
			  "Quantity": 1
			},
			{
			  "Name": "Test",
			  "Price": 0,
			  "Quantity": 2
			}
		  ]
		}
	  }
	]
  }
  ```

 - **Delete**:
  `DELETE https://localhost:5000/api/Base?id=`
  Belirli bir kaydı silmek için kayıt kimliğini parametre olarak belirtin.

## Örnek Kullanım Senaryoları

1. **Müşteri ve Sipariş Oluşturma**:
   - Müşteri bilgilerini DynamicObject bölümünde ve sipariş detaylarını DynamicSubObject bölümünde göndererek istek yapın.

2. **Müşterinin Siparişlerini Okuma**:
   - Müşteri kimliğine göre tüm siparişleri getirmek için bir GET isteği gönderin.

3. **Bilgileri Güncelleme**:
   - Mevcut bir siparişteki ürünlerin fiyatını veya miktarını güncellemek için güncellenmiş JSON verilerini içeren bir PUT isteği gönderin.

4. **Bir Kaydı Silme**:
   - Bir müşteri, sipariş veya ürünü silmek için kimliği DELETE isteğinde sağlayın.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------

# Dynamic Object Creation API

## Overview

This API provides a system where users can create, read, update, and delete various objects (e.g., Orders, Products, Customers) through a single dynamic table. The API dynamically manages different object types and their fields, eliminating the need to manually create physical database tables. All CRUD operations are performed through a central API gateway, and the system ensures that related objects (e.g., an Order and its Products) are processed as a whole.

## Features

1. **Dynamic Object Creation**:
   - Users can dynamically create new objects (e.g., orders, customers, products) by sending a request to the API.
   - The structure of each object is stored in a central table, providing flexibility in object types and fields.

2. **CRUD Operations**:
   - Perform dynamic object creation, reading, updating, and deletion operations through a single API gateway.
   - The API accommodates different object types (e.g., orders, products, customers) and accepts dynamic fields for each object.

3. **Transaction Management**:
   - An API request treats multiple related objects (e.g., an Order and its Products) as a single transaction.
   - Transaction management is applied to ensure that all objects are successfully created or none are created in case of an error.

4. **Error Handling**:
   - Manages scenarios such as invalid object structure, missing fields, database connection issues, and validation errors.
   - Provides meaningful error messages that explain the cause of the error to users.

5. **Data Validation**:
   - Validates that required fields are present for each object type.
   - Validation logic is tailored to the object type (e.g., Orders require a customer ID and at least one product, while Products require a name and price).

## API Endpoints

- **Create**:
  `POST https://localhost:5000/api/Base`
  Send data in JSON format to create new records.
  Example JSON request:
  ```json
  {
	"DynamicObject": {
	  "Customer": {
		"CustomerId": "11111",
		"Name": "test user",
		"Password": "12345"
	  }
	},
	"DynamicSubObject": [
	  {
		"Order": {
		  "CustomerId": "11111",
		  "Product": [
			{
			  "Name": "Test",
			  "Price": 0,
			  "Quantity": 1
			},
			{
			  "Name": "Test",
			  "Price": 0,
			  "Quantity": 2
			}
		  ]
		}
	  }
	]
  }
  ```

- **Read**:
  `Get https://localhost:5000/api/Base`
  Fetch records based on object type and filters (e.g., get all orders for a specific customer).

- **Update**:
  `PUT https://localhost:5000/api/Base`
  Send updated data in JSON format to update existing records.
  Example JSON request:
  ```json
  {
	"id": 1, 
	"DynamicObject": {
	  "Customer": {
		"CustomerId": "11111",
		"Name": "test user",
		"Password": "12345"
	  }
	},
	"DynamicSubObject": [
	  {
		"Order": {
		  "CustomerId": "11111",
		  "Product": [
			{
			  "Name": "Test",
			  "Price": 0,
			  "Quantity": 1
			},
			{
			  "Name": "Test",
			  "Price": 0,
			  "Quantity": 2
			}
		  ]
		}
	  }
	]
  }
  ```

- **Delete**:
  `DELETE https://localhost:5000/api/Base?id=`
  Specify the record ID as a parameter to delete a specific record.

## Example Use Cases

1. **Creating Customers and Orders**:
   - Make a request by sending customer details in the DynamicObject section and order details in the DynamicSubObject section.

2. **Reading a Customer's Orders**:
   - Send a GET request to fetch all orders for a customer based on their ID.

3. **Updating Information**:
   - Send a PUT request with updated JSON data to modify the price or quantity of products in an existing order.

4. **Deleting a Record**:
   - Provide the ID to delete a customer, order, or product.