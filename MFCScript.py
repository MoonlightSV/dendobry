import pyodbc
import numpy as np
server = r'LAPTOP-U4B7KRUU\SQLEXPRESS'
database = 'AIS_STAT_Training'
username = 'sa'
password = '123456'
conn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER='+server+';DATABASE='+database+';UID='+username+';PWD='+password)
cursor = conn.cursor()

cursor.execute("""
	select distinct service, custom_service_id from AIS_cpgu_order
""")

service_list = []

for row in cursor.fetchall():
	service_list.append(tuple(row))

count = len(service_list)

print(count)

f = open("service.txt", "w")
for i in range(count):
	f.write(str(service_list[i][0]) + ' ')
f.write('\n')
for i in range(count):
	f.write(str(service_list[i][1]) + ' ')
f.close()

matrix = np.zeros((count, count))

cursor.execute("""
	select distinct top 100 requester from AIS_cpgu_order where close_date is not NULL
""")

requester_list = []

for row in cursor.fetchall():
	requester_list.append(row)

for requester in requester_list:
	exec_str = """
		select service, custom_service_id, close_date 
		from AIS_cpgu_order 
		where requester = {}
		order by close_date 
	""".format(str(requester[0]))
	cursor.execute(exec_str)
	order_list = []
	for order in cursor.fetchall():
		elem = (order[0], order[1])
		order_list.append(service_list.index(elem))	
	for order in order_list[0:-2]:
		for i in range(order_list.index(order) + 1, len(order_list)):
			matrix[order][order_list[i]] += 1

f = open('matrix.txt', 'w')

for i in range(count):
	for j in range(count):
		f.write(str(matrix[i][j]) + ' ')
	f.write('\n')

f.close()	