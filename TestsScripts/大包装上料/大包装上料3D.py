import time

DB_INDEX= 200
S7.WriteShort(DB_INDEX,12,2)
S7.WriteBit(DB_INDEX, 10,0,True)
time.sleep(3)
S7.WriteShort(DB_INDEX,12,0)
S7.WriteBit(DB_INDEX, 10,0,False)