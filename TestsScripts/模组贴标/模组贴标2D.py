import time

DB_INDEX= 200
S7.WriteShort(DB_INDEX,12,1)
S7.WriteShort(DB_INDEX,14 ,2)
S7.WriteUInt32(DB_INDEX,16 ,3)
S7.WriteBit(DB_INDEX, 10,0,True)
time.sleep(3)
S7.WriteShort(DB_INDEX,12,0)
S7.WriteShort(DB_INDEX,14 ,0)
S7.WriteUInt32(DB_INDEX,16 ,0)
S7.WriteBit(DB_INDEX, 10,0,False)