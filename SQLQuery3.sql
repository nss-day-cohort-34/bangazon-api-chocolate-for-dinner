SELECT o.Id, o.CustomerId, o.PaymentTypeId, 
                                       c.Id AS CustomerId, c.FirstName, c.LastName
                                       FROM [Order] o
                                       LEFT JOIN Customer c ON c.Id = o.CustomerId
									   WHERE o.id = 6

            