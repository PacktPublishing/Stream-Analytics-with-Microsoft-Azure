//Fraud Detection ASA job Query

SELECT System.Timestamp AS WindowEnd, COUNT(*) AS FraudulentCalls
 INTO "CallStream-PowerBI"
 FROM "CallStream" CS1 TIMESTAMP BY CallRecTime
 JOIN "CallStream" CS2 TIMESTAMP BY CallRecTime

 /* Where the caller is the same, as indicated by IMSI (International Mobile Subscriber Identity) */
 ON CS1.CallingIMSI = CS2.CallingIMSI

 /* ...and date between CS1 and CS2 is between one and five seconds */
 AND DATEDIFF(ss, CS1, CS2) BETWEEN 1 AND 5 

 /* Where the switch location is different */
 WHERE CS1.SwitchNum != CS2.SwitchNum
 GROUP BY TumblingWindow(Duration(second, 1)) 