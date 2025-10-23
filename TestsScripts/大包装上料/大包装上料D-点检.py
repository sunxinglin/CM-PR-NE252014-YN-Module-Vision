import time

DB_INDEX= 200

for i in range(1,7):
    S7.WriteShort(DB_INDEX,56,i+1)
    if(i==6):
        S7.WriteBit(DB_INDEX, 54,1,True)
    else:
        S7.WriteBit(DB_INDEX, 54,0,True)
        
    time.sleep(3)
    
    S7.WriteShort(DB_INDEX,56,0)
    S7.WriteBit(DB_INDEX, 54,0,False)
    S7.WriteBit(DB_INDEX, 54,1,False)
    
    time.sleep(3)
    