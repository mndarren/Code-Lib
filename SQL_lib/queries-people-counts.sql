

SELECT COUNT (*) FROM postaddr.people_postal_addresses;  -- 195,494

SELECT COUNT (*) FROM people.people;  -- 330,137

SELECT COUNT (*) FROM postaddr.postal_addresses;  -- 236,000

SELECT COUNT (p.id) FROM
	people.people AS p
WHERE NOT EXISTS (
	SELECT person 
	FROM postaddr.people_postal_addresses
	WHERE person=p.id
);  -- 189,288

SELECT COUNT (p.id) FROM
	people.people AS p
WHERE EXISTS (
	SELECT person 
	FROM postaddr.people_postal_addresses
	WHERE person=p.id
);  -- 140,909


SELECT COUNT (*) FROM orgs.orgs;  -- 107,832


SELECT COUNT (o.id) FROM
	orgs.orgs AS o
WHERE NOT EXISTS (
	SELECT org 
	FROM postaddr.orgs_postal_addresses
	WHERE org=o.id
);  -- 67,780


SELECT COUNT (o.id) FROM
	orgs.orgs AS o
WHERE EXISTS (
	SELECT org 
	FROM postaddr.orgs_postal_addresses
	WHERE org=o.id
);  -- 40,052




SELECT COUNT (p.id) FROM
	people.people AS p
WHERE
( 
	EXISTS (
		SELECT person 
		FROM postaddr.people_postal_addresses
		WHERE person=p.id
	)
	AND EXISTS (
		SELECT person
		FROM people.people_external_identifiers
		WHERE person=p.id AND external_data_source='ITI'
	)
);  -- 3,826

SELECT COUNT (p.id) FROM
	people.people AS p
WHERE
( 
	EXISTS (
		SELECT person 
		FROM postaddr.people_postal_addresses
		WHERE person=p.id
	)
	AND EXISTS (
		SELECT person
		FROM people.people_external_identifiers
		WHERE person=p.id AND external_data_source='IL'
	)
);  -- 137,685

select c.name, c.state, s.code, pz.postal_code, s.name
from postaddr.postal_zones as pz inner join
     postaddr.cities as c on c.id = pz.city inner join
     postaddr.states as s on c.state = s.code 
where  pz.postal_code in ('47166', '07446', '62080')
order by c.name;

select pz.postal_code
from postaddr.postal_zones as pz inner join
     postaddr.cities as c on c.id = pz.city
where c.name = 'Ramsey';

-- find all cities with name 'Ramsey'
select c.name, c.state, s.code, pz.postal_code, s.name
from postaddr.postal_zones as pz inner join
     postaddr.cities as c on c.id = pz.city inner join
     postaddr.states as s on c.state = s.code 
where  c.name = 'Ramsey'
order by c.name;

-- count cities with the same zip code
select count(pz.postal_code) as count, pz.postal_code
from postaddr.postal_zones as pz inner join
     postaddr.cities as c on c.id = pz.city inner join
     postaddr.states as s on c.state = s.code 
group by pz.postal_code
order by count;

-- get same zip code related different cities
WITH pzcounts AS 
	(
	SELECT 
		pz.postal_code as postal_code, 
		count(pz.postal_code) AS count
	FROM 
		postaddr.postal_zones AS pz
	GROUP BY pz.postal_code
	)
SELECT 
	pz.postal_code, 
	pzcounts.count as pzcount, 
	c.name as city, 
	s.name as state
FROM 
	postaddr.postal_zones AS pz
	INNER JOIN pzcounts
		ON pzcounts.postal_code = pz.postal_code
	INNER JOIN postaddr.cities AS c
		ON pz.city = c.id
	INNER JOIN postaddr.states AS s
		ON c.state = s.code
ORDER BY pzcount DESC, postal_code;
