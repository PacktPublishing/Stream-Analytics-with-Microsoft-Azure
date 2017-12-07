--UPPER
select ehinput.timestamp, ehinput.vin, ehinput.speed, ehinput.outsidetemperature,
    UPPER(ehinput.city) as city
from ehinput


--AVG
select System.Timestamp as WindowEndTimestamp, ehinput.city,
    AVG(ehinput.speed) as average_speed
from ehinput timestamp by ehinput.timestamp
group by ehinput.city, TumblingWindow(minute, 1)


--COLLECT
select COLLECT()
from ehinput timestamp by ehinput.timestamp
group by TumblingWindow(second, 30)


--TopOne
select ehinput.city, System.Timestamp as WindowEndTimestamp,
    TopOne() OVER (ORDER BY ehinput.speed DESC) as Fastest
from ehinput timestamp by ehinput.timestamp
group by ehinput.city, TumblingWindow(minute, 1)


--ISFIRST
select ehinput.city, ehinput.timestamp, ehinput.vin, ehinput.speed,
    ISFIRST(mi, 10) OVER(PARTITION BY ehinput.city when ehinput.speed > 70) as first_over_70
from ehinput


--LAST
select ehinput.city, ehinput.timestamp, ehinput.vin, ehinput.speed,
    LAST(ehinput.vin) OVER(PARTITION BY ehinput.city limit duration(hour, 24) when ehinput.speed > 70) as last_car_over_70
from ehinput


--LAG
select ehinput.vin, ehinput.city, ehinput.timestamp, ehinput.speed,
    ehinput.speed - 
    LAG(ehinput.speed) OVER(PARTITION BY ehinput.city, ehinput.vin limit duration(hour, 24)) as speed_change
from ehinput


--GetArrayElement
select GetArrayElement(ehinput.sensors, 0) as firstSensor,
    GetArrayElement(ehinput.sensors, 1) as secondSensor
from ehinput


--GetArrayLength
select ehinput.conferenceRoom,
    GetArrayLength(ehinput.sensors) as countOfMotionSensors
from ehinput


--GetArrayElements
select ehinput.conferenceRoom,
    arrayElement.arrayIndex as sensorIndex,
    arrayElement.arrayValue as sensorValue
from ehinput
CROSS APPLY GetArrayElements(ehinput.sensors) as arrayElement


--access array element with dot notation
select conferenceRoom,
    readings.temperature
from ehinput


--access all array elements with *
select conferenceRoom,
    readings.*
from ehinput


--WITH
WITH Speeding AS
(
select *
from eventhubsource
where speed > 70
)

select city, vin, timestamp, speed
into AllSpeeding 
from Speeding

select city, count(vin) as speedsters
into SpeedstersByCity
from Speeding 


--JOIN streams
with 
Redmond as
(select * from EventHubSource timestamp by eventhubsource.timestamp where city = 'Redmond'),
Seattle as
(select * from EventHubSource timestamp by eventhubsource.timestamp where city = 'Seattle')

select Redmond.vin as vin1, Seattle.vin as vin2
from Redmond
JOIN Seattle
ON Redmond.vin = Seattle.vin
and DATEDIFF(mi, Redmond, Seattle) between 0 and 10


--JOIN reference data
select EventHubSource.vin, BlobSource.Model, EventHubSource.timestamp, EventHubSource.outsideTemperature
into EventHubOut
from EventHubSource
JOIN BlobSource on EventHubSource.vin = BlobSource.VIN


--TumblingWindow
select System.Timestamp as WindowEndTimestamp, ehinput.city,
    avg(ehinput.speed) as average_speed,
    count(ehinput.vin) as count_of_cars
from ehinput timestamp by ehinput.timestamp
group by ehinput.city, TumblingWindow(second, 10)


--HoppingWindow
select System.Timestamp as WindowEndTimestamp, ehinput.city,
    avg(ehinput.speed) as average_speed,
    count(ehinput.vin) as count_of_cars
from ehinput timestamp by ehinput.timestamp
group by ehinput.city, HoppingWindow(Duration(second, 10), Hop(second, 2))


--SlidingWindow
select System.Timestamp as WindowEndTimestamp, ehinput.city,
    avg(ehinput.speed) as average_speed,
    count(ehinput.vin) as count_of_cars
from ehinput timestamp by ehinput.timestamp
group by ehinput.city, SlidingWindow(second, 10)


--System.Timestamp
select System.Timestamp as event_timestamp,
    ehinput.city,
    ehinput.speed
from ehinput timestamp by ehinput.timestamp


--TIMESTAMP BY
select System.Timestamp as event_timestamp,
    ehinput.city,
    ehinput.speed
from ehinput TIMESTAMP BY ehinput.timestamp


