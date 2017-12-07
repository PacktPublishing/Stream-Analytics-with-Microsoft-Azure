create table input
(ID BigINT,
Coordinate_X nvarchar(MAX),
Coordinate_Y nvarchar(MAX),
Coordinate_Z nvarchar(MAX))
Select count(input.ID) as SensorID,
UDF.ConvertToInt(input.Coordinate_X) as X_CoOrdinate, 
UDF.ConvertToInt(input.Coordinate_Y) as Y_CoOrdinate,
UDF.ConvertToInt(input.Coordinate_Z) as Z_CoOrdinate
into outputblob from input GROUP BY input.Coordinate_X, input.Coordinate_Y, input.Coordinate_Z, 
tumblingWindow(Second,1)
 