using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWordleClient
{
    public record WordResult(bool isCorrect, List<Color> colorCodes);
}
