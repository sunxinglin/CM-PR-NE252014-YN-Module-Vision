import time

DB_INDEX= 200
S7.WriteShort(DB_INDEX,100,1)
S7.WriteShort(DB_INDEX,102 ,2)
S7.WriteUInt32(DB_INDEX,104 ,3)
S7.WriteBit(DB_INDEX, 98,0,True)
time.sleep(3)
S7.WriteShort(DB_INDEX,100,0)
S7.WriteShort(DB_INDEX,102 ,0)
S7.WriteUInt32(DB_INDEX,104 ,0)
S7.WriteBit(DB_INDEX, 98,0,False)