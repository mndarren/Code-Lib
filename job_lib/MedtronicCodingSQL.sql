-- Query to return a count
SELECT Count(p.Patientkey)
FROM Patien_Address pa
	 RIGHT OUTER JOIN Patient p ON p.Patientkey = pa.PatientKey
	 LEFT OUTER JOIN Encounter e ON e.PatientKey = p.Patientkey
WHERE pa.Country = 'US' 
	  AND pa.CounterName != NULL 
	  AND e.EncounterDate < Date(2019 last date)
	  AND e.EncounterDate > Date(2019 first date)
-- last year needs to be specific: physical year or financial year