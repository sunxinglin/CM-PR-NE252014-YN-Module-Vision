import time

DB_INDEX= 200
S7.WriteShort(DB_INDEX,112,5)
S7.WriteShort(DB_INDEX,114 ,6)
S7.WriteBit(DB_INDEX, 110,0,True)
time.sleep(3)
S7.WriteShort(DB_INDEX,112,0)
S7.WriteShort(DB_INDEX,114 ,0)
S7.WriteBit(DB_INDEX, 110,0,False)