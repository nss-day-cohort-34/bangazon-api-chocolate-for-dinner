 ALTER TABLE Product
              ADD IsDeleted BIT;

 ALTER TABLE Computer
              ADD IsDeleted BIT;

ALTER TABLE TrainingProgram
              ADD IsDeleted BIT;

ALTER TABLE ProductType
              ADD IsDeleted BIT;

ALTER TABLE PaymentType
              ADD IsDeleted BIT;

ALTER TABLE [Order]
              ADD IsDeleted BIT;