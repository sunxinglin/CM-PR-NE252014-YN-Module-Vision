import time

DB_INDEX= 200
S7.WriteShort(DB_INDEX,62,5)
S7.WriteShort(DB_INDEX,64 ,6)
S7.WriteBit(DB_INDEX, 60,0,True)
time.sleep(3)
S7.WriteShort(DB_INDEX,62,0)
S7.WriteShort(DB_INDEX,64 ,0)
S7.WriteBit(DB_INDEX, 60,0,False)