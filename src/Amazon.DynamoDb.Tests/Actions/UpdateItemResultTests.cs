﻿using System.Text.Json;

namespace Amazon.DynamoDb.Models.Tests;

public class UpdateItemResultTests
{
    [Fact]
    public void ComplexAttributeCollection()
    {
        var json = """{"Attributes":{"PP":{"B":"AA=="},"CST":{"BOOL":false},"PT":{"N":"0"},"LR":{"B":"AA=="},"SRol":{"L":[{"S":"NULL"}]},"ODPr":{"L":[{"M":{"Pro":{"N":"-1"},"Slo":{"N":"0"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"1"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"2"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"3"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"4"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"5"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"6"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"7"}}}]},"HS":{"S":"08d40421-871b-4bc2-9c5d-d0915cd9a839"},"DDPr":{"L":[{"M":{"Pro":{"N":"-1"},"Slo":{"N":"0"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"1"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"2"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"3"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"4"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"5"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"6"}}},{"M":{"Pro":{"N":"-1"},"Slo":{"N":"7"}}}]},"SOI":{"N":"0"},"CVC":{"N":"0"},"FT":{"N":"0"},"DDCa":{"L":[{"S":"NULL"}]},"CCT":{"L":[{"S":"NULL"}]},"UF":{"N":"0"},"FTL":{"L":[{"S":"NULL"}]},"US":{"B":"AA=="},"A":{"BOOL":false},"SR":{"N":"1"},"C":{"N":"0"},"DDE":{"N":"0"},"F":{"L":[{"S":"NULL"}]},"LasT":{"N":"0"},"CFP":{"N":"0"},"CP":{"N":"0"},"VC":{"N":"0"},"DvD":{"N":"18463"},"CT":{"N":"0"},"SPR":{"L":[{"S":"NULL"}]},"PF":{"N":"0"},"SPT":{"M":{"NULL":{"S":"NULL"}}},"EXPr":{"L":[{"M":{"T":{"N":"3"},"V":{"B":"AA=="},"Ve":{"B":"AA=="}}}]},"VP":{"L":[{"M":{"I":{"B":"AQ=="},"C":{"B":"AA=="},"L":{"N":"0"},"F":{"N":"1595261267"}}},{"M":{"I":{"B":"AA=="},"C":{"B":"AA=="},"L":{"N":"0"},"F":{"N":"1595261270"}}},{"M":{"I":{"B":"Ag=="},"C":{"B":"AA=="},"L":{"N":"0"},"F":{"N":"0"}}}]},"Id":{"S":"41551"},"RM":{"BOOL":false},"PM":{"BOOL":false}}}""";

        var attrCollection = JsonSerializer.Deserialize<UpdateItemResult>(json).Attributes;

        Assert.Equal(-1, (attrCollection["ODPr"].ToArray<DbValue>()[0].Value as AttributeCollection)["Pro"].ToInt());
    }

    [Fact]
    public void ComplexAttributeCollection2()
    {
        var json = """{"Attributes":{"LSI":{"N":"0"},"Qr":{"B":"AA=="},"DN":{"BOOL":true},"HS":{"S":"0413eeb6-d30e-4384-a753-10921f3b57e0"},"GIT":{"S":"NULL"},"v":{"BOOL":true},"CNC":{"N":"0"},"Pe":{"N":"0"},"New":{"BOOL":true},"Name":{"S":"NULL"},"SQ":{"N":"1595261265"},"GId":{"N":"-1"},"CD":{"N":"1595275664"},"D":{"N":"0"},"E":{"L":[{"N":"12"},{"N":"13"},{"N":"1"},{"N":"3"},{"N":"24"},{"N":"21"}]},"Ps":{"L":[{"S":"NULL"}]},"AE":{"N":"0"},"GL":{"N":"0"},"Pay":{"BOOL":false},"LJG":{"S":"NULL"},"M":{"N":"0"},"LJM":{"S":"NULL"},"Ds":{"L":[{"M":{"P":{"M":{"NULL":{"S":"NULL"}}},"I":{"L":[{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"}]},"C":{"L":[{"N":"2"},{"N":"88"},{"N":"89"},{"N":"46"},{"N":"1"},{"N":"6"},{"N":"14"}]},"T":{"N":"0"}}},{"M":{"P":{"M":{"NULL":{"S":"NULL"}}},"I":{"L":[{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"}]},"C":{"L":[{"N":"2"},{"N":"88"},{"N":"89"},{"N":"46"},{"N":"1"},{"N":"6"},{"N":"14"}]},"T":{"N":"1"}}},{"M":{"P":{"M":{"NULL":{"S":"NULL"}}},"I":{"L":[{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"}]},"C":{"L":[{"N":"2"},{"N":"88"},{"N":"89"},{"N":"46"},{"N":"1"},{"N":"6"},{"N":"14"}]},"T":{"N":"2"}}},{"M":{"P":{"M":{"NULL":{"S":"NULL"}}},"I":{"L":[{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"}]},"C":{"L":[{"N":"2"},{"N":"88"},{"N":"89"},{"N":"46"},{"N":"1"},{"N":"6"},{"N":"14"}]},"T":{"N":"3"}}},{"M":{"P":{"M":{"NULL":{"S":"NULL"}}},"I":{"L":[{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"}]},"C":{"L":[{"N":"2"},{"N":"88"},{"N":"89"},{"N":"46"},{"N":"1"},{"N":"6"},{"N":"14"}]},"T":{"N":"4"}}},{"M":{"P":{"M":{"NULL":{"S":"NULL"}}},"I":{"L":[{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"},{"N":"-1"}]},"C":{"L":[{"N":"2"},{"N":"88"},{"N":"89"},{"N":"46"},{"N":"1"},{"N":"6"},{"N":"14"}]},"T":{"N":"5"}}}]},"Q":{"L":[{"S":"NULL"}]},"CS":{"L":[{"S":"NULL"}]},"Sc":{"N":"0"},"AS":{"N":"0"},"Id":{"S":"41551"},"LJ":{"N":"0"}}}""";

        var attrCollection = JsonSerializer.Deserialize<UpdateItemResult>(json).Attributes;

        var ds0 = attrCollection["Ds"].ToArray<DbValue>()[0].Value as AttributeCollection;

        DbValue[] cArray = ds0["C"].ToArray<DbValue>();

        Assert.Equal(2, cArray[0].ToInt());
        Assert.Equal(88, cArray[1].ToInt());
        Assert.Equal(89, cArray[2].ToInt());
        Assert.Equal(46, cArray[3].ToInt());
    }
}