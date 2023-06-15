using System.Collections.ObjectModel;

namespace Documently.Models;

/*
 * Интерфейс для взаимодействия с процессором шаблонов
 * Реализуйте его в своём бэкенде
 */
interface ITemplateProcessor
{
    /*
     * Настроить окружение процессора
     * name    - путь к файлу шаблона, который заполняется
     * path    - путь к папке, куда необходимо записывать готовые документы
     * pattern - шаблон имени результирующего файла
     */
    public void Setup (string name, string path, string pattern);

    /*
     * Извлечь список полей из шаблона
     */
    public ObservableCollection<Field> GetFields ();

    /*
     * Заполнить шаблон
     * record - набор значений для одного экземпляра
     */
    public void Fill (ObservableCollection<Field> record);

    /*
     * Освободить ресурсы, занятые процессором
     * Сюда входит какая-либо память, файловые потоки и прочее
     */
    public void Dispose ();
}