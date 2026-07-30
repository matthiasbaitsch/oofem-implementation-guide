using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public static class ExcelReader
{
    public static Structure Read(string file)
    {
        XSSFWorkbook workbook = new XSSFWorkbook(file);
        workbook.MissingCellPolicy = MissingCellPolicy.CREATE_NULL_AS_BLANK;

        // Structure
        Structure structure = new Structure();

        // nodes
        ISheet nodesSheet = workbook.GetSheetAt(0);
        for (int i = 1; i <= nodesSheet.LastRowNum; i++)
        {
            IRow row = nodesSheet.GetRow(i);
            Node node = structure.AddNode(
                row.GetCell(0).NumericCellValue,
                row.GetCell(1).NumericCellValue
            );
            node.Constraint.Fixed[0] = row.GetCell(2).BooleanCellValue;
            node.Constraint.Fixed[1] = row.GetCell(3).BooleanCellValue;
            node.Force.Components[0] = row.GetCell(4).NumericCellValue;
            node.Force.Components[1] = row.GetCell(5).NumericCellValue;
        }

        // elements
        ISheet elementsSheet = workbook.GetSheetAt(1);
        for (int i = 1; i <= elementsSheet.LastRowNum; i++)
        {
            IRow row = elementsSheet.GetRow(i);
            structure.AddElement(
                row.GetCell(0).NumericCellValue,
                row.GetCell(1).NumericCellValue,
                (int)row.GetCell(2).NumericCellValue,
                (int)row.GetCell(3).NumericCellValue
            );
        }
        return structure;
    }
}