namespace MicromarinCase.JsonProcessing
{
    public class JsonValidator
    {
        public void ValidateJson(object jsonData)
        {
            if (jsonData is not IDictionary<string, object> jsonDictionary)
            {
                throw new ArgumentException("Invalid JSON data format.");
            }

            if (jsonDictionary.ContainsKey("DynamicObject"))
            {
                var dynamicObject = jsonDictionary["DynamicObject"] as IDictionary<string, object>;
                if (dynamicObject.Count == 0)
                {
                    throw new ArgumentException("The DynamicObject field is not in a valid JSON format.");
                }

                if (dynamicObject.ContainsKey("Customer"))
                {
                    var customerData = dynamicObject["Customer"] as IDictionary<string, object>;
                    if (customerData == null)
                    {
                        throw new ArgumentException("The Customer field is not in a valid JSON format.");
                    }

                    var requiredCustomerFields = new List<string> { "CustomerId", "Name", "Password" };
                    foreach (var field in requiredCustomerFields)
                    {
                        if (!customerData.ContainsKey(field))
                        {
                            throw new ArgumentException($"Customer field is missing: '{field}' is required.");
                        }
                    }

                    if (jsonDictionary.ContainsKey("DynamicSubObject"))
                    {
                        var dynamicSubObject = jsonDictionary["DynamicSubObject"] as List<object>;
                        if (dynamicSubObject.Count == 0)
                        {
                            throw new ArgumentException("The DynamicSubObject field is not in a valid JSON format.");
                        }

                        foreach (var subObject in dynamicSubObject)
                        {
                            var orderData = (subObject as IDictionary<string, object>)?["Order"] as IDictionary<string, object>;
                            if (orderData == null)
                            {
                                throw new ArgumentException("Each DynamicSubObject must contain a valid Order field.");
                            }

                            var requiredOrderFields = new List<string> { "CustomerId" };
                            foreach (var field in requiredOrderFields)
                            {
                                if (!orderData.ContainsKey(field))
                                {
                                    throw new ArgumentException($"Order field is missing: '{field}' is required.");
                                }
                            }

                            if (orderData.ContainsKey("Product"))
                            {
                                var productData = orderData["Product"] as List<object>;
                                if (productData == null)
                                {
                                    throw new ArgumentException("The Product field is not in a valid JSON format.");
                                }

                                foreach (var product in productData)
                                {
                                    var productFields = product as IDictionary<string, object>;
                                    if (productFields == null)
                                    {
                                        throw new ArgumentException("The elements in Product are not in a valid JSON format.");
                                    }

                                    var requiredProductFields = new List<string> { "Name", "Price", "Quantity" };
                                    foreach (var field in requiredProductFields)
                                    {
                                        if (!productFields.ContainsKey(field))
                                        {
                                            throw new ArgumentException($"Product field missing: '{field}' is required.");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new ArgumentException("Each Order must contain at least one Product field.");
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("The DynamicObject containing a Customer must contain a DynamicSubObject containing at least an Order and a Product.");
                    }
                }

                if (dynamicObject.ContainsKey("Order"))
                {
                    var orderData = dynamicObject["Order"] as IDictionary<string, object>;
                    if (orderData == null)
                    {
                        throw new ArgumentException("The Order field is not in a valid JSON format.");
                    }

                    if (!orderData.ContainsKey("CustomerId"))
                    {
                        throw new ArgumentException("Order field must contain 'CustomerId'.");
                    }

                    if (jsonDictionary.ContainsKey("DynamicSubObject"))
                    {
                        var dynamicSubObject = jsonDictionary["DynamicSubObject"] as List<object>;
                        if (dynamicSubObject.Count == 0)
                        {
                            throw new ArgumentException("The DynamicSubObject field is not in a valid JSON format.");
                        }

                        foreach (var subObject in dynamicSubObject)
                        {
                            var productData = (subObject as IDictionary<string, object>)?["Product"] as List<object>;
                            if (productData == null)
                            {
                                throw new ArgumentException("DynamicSubObject must contain a valid Product field.");
                            }

                            foreach (var product in productData)
                            {
                                var productFields = product as IDictionary<string, object>;
                                if (productFields == null)
                                {
                                    throw new ArgumentException("The elements in Product are not in a valid JSON format.");
                                }

                                var requiredProductFields = new List<string> { "Name", "Price", "Quantity" };
                                foreach (var field in requiredProductFields)
                                {
                                    if (!productFields.ContainsKey(field))
                                    {
                                        throw new ArgumentException($"Product field missing: '{field}' is required.");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("DynamicObject with an Order must contain a DynamicSubObject with at least one Product.");
                    }
                }
            }
            else
            {
                throw new ArgumentException("JSON must contain a DynamicObject field.");
            }
        }
    }
}
