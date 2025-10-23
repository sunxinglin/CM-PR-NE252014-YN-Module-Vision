import time

DB_INDEX= 200
S7.WriteShort(DB_INDEX,150,5)
S7.WriteShort(DB_INDEX,152 ,6)
S7.WriteBit(DB_INDEX, 148,0,True)
time.sleep(3)
S7.WriteShort(DB_INDEX,150,0)
S7.WriteShort(DB_INDEX,152 ,0)
S7.WriteBit(DB_INDEX, 148,0,False)